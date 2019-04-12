# HW4 实验报告

516030910381 姜凡叙

### Cook-Torrance模型的实现

这部分的工作为补充`Schlick_F()`、`GGX_D()`和`CookTorrence_G()`三个函数，分别根据给出的三个公式实现

- Distribution term

  根据公式$D_{GGX} = \frac{\alpha ^ 2}{\pi ((n\cdot h)^2(\alpha^2 - 1) + 1)^2}​$

  得到如下代码：

  ```c#
  float GGX_D(float roughness, float NdotH)
  {
      float D;
      D = pow(roughness, 2) / (UNITY_PI * pow(pow(NdotH, 2) * (pow(roughness, 2) - 1) + 1, 2));
      return D;
  }
  ```

  其中，程序中的`roughness`是公式中的$\alpha​$。这样便得到了​$D_{GGX}​$项，能够看到高光效果。

- Fresnel term

  根据公式$F_{schlick} = C_{spec} + (1-C_{spec})(1-l\cdot h)^5​$

  得到如下代码：

  ```c#
  float3 Schlick_F(half3 R, half cosA)
  {
      float3 F;
      F = R + (1 - R) * pow((1 - cosA), 5);
      return F;
  }
  ```

  其中，$R$为利用 `DiffuseAndSpecularFromMetallic`计算得到的$C_{spec}$项，`cosA`即为$l\cdot h$。这样便得到了$F_{Schlick}$项。

- Geometry term

  根据Cook-Torrance模型$G_{Cook-Torrance} = min(1, \frac{2(n\cdot h)(n\cdot v)}{v\cdot h}, \frac{2(n\cdot h)(n\cdot l)}{v\cdot h})​$

  得到如下代码：

  ```c#
  float CookTorrence_G (float NdotL, float NdotV, float VdotH, float NdotH)
  {
      float G;
      float res1, res2;
      res1 = 2 * NdotH * NdotV / VdotH;
      res2 = 2 * NdotH * NdotL / VdotH;
      G = res1 < res2 ? res1 : res2;
      G = G < 1 ? G : 1;
      return G;
  }
  ```

  这样便得到了$G_{Cook-Torrance}$项。

- 其他工作

  原有的代码已经完成了之后对上面三项的整合，并得到了最终的结果`color`项，我们选择返回`color`。

  关于材质球矩阵的生成，原有代码也已经实现了，调整camera位置，运行场景，便得到了如下的结果：

  ![](./pictures/BRDF.jpg)

### 素描风格渲染

根据论文中的描述，在素描风格渲染的时候，采用一个TAM来进行渲染，这样不仅节省资源，还有渐变的较好的效果。于是我找到了论文中Figure 2中的6张图片作为渲染中使用的纹理来进行渲染。

![](./pictures/Texture.jpg)

首先需要计算物体的漫反射，然后根据计算漫反射得到的物体的明暗程度来确定上面6个纹理在物体的某个位置的权重值，越亮的地方线条越稀疏，越暗的地方线条越密集。

```c#
if (hatchFactor > 6.0) 
{
    // white
}
else if (hatchFactor > 5.0) 
{
    o.hatchWeights0.x = hatchFactor - 5.0;
}
else if (hatchFactor > 4.0)
{
    o.hatchWeights0.x = hatchFactor - 4.0;
	o.hatchWeights0.y = 1.0 - o.hatchWeights0.x;
}
else if (hatchFactor > 3.0) 
{
	o.hatchWeights0.y = hatchFactor - 3.0;
	o.hatchWeights0.z = 1.0 - o.hatchWeights0.y;
}
else if (hatchFactor > 2.0) 
{
	o.hatchWeights0.z = hatchFactor - 2.0;
	o.hatchWeights1.x = 1.0 - o.hatchWeights0.z;
}
else if (hatchFactor > 1.0) 
{
	o.hatchWeights1.x = hatchFactor - 1.0;
	o.hatchWeights1.y = 1.0 - o.hatchWeights1.x;
}
else 
{
	o.hatchWeights1.y = hatchFactor;
	o.hatchWeights1.z = 1.0 - o.hatchWeights1.y;
}
```

其中`hatchWeights0`与`hatchWeights1`用来记录权重信息。之后在FragmentProgram中根据得到的权重将纹理叠加起来便初步得到了素描风格的渲染效果。

```c#
fixed4 hatchTex0 = tex2D(_Hatch0, i.uv) * i.hatchWeights0.x;
fixed4 hatchTex1 = tex2D(_Hatch1, i.uv) * i.hatchWeights0.y;
fixed4 hatchTex2 = tex2D(_Hatch2, i.uv) * i.hatchWeights0.z;
fixed4 hatchTex3 = tex2D(_Hatch3, i.uv) * i.hatchWeights1.x;
fixed4 hatchTex4 = tex2D(_Hatch4, i.uv) * i.hatchWeights1.y;
fixed4 hatchTex5 = tex2D(_Hatch5, i.uv) * i.hatchWeights1.z;
fixed4 whiteColor = fixed4(1, 1, 1, 1) * (1 - i.hatchWeights0.x - i.hatchWeights0.y - i.hatchWeights0.z - i.hatchWeights1.x - i.hatchWeights1.y - i.hatchWeights1.z);

fixed4 hatchColor = hatchTex0 + hatchTex1 + hatchTex2 + hatchTex3 + hatchTex4 + hatchTex5 + whiteColor;
```

然后我加入了描边的处理，新写一个Pass，在里面首先声明`Cull Front`来剔除正面，只渲染背面。然后将表面以及表面微小高度间的空间渲染成黑色，这样从正面看便像是有了描边的效果。VertexProgram中的代码如下：

```c#
FragmentData MyVertexProgram(VertexData v)
{
    FragmentData o;
    o.position = UnityObjectToClipPos(v.position);
    float3 vnormal = mul((float3x3)UNITY_MATRIX_IT_MV, v.normal);
	float2 offset = TransformViewToProjection(vnormal.xy);
	o.position.xy += offset * _Outline;
	return o;
}
```

然后在FragmentProgram中写入`return float4(0, 0, 0, 1)`便完成了。最终实现的效果如下：

![](./pictures/Sketch.jpg)

### 运动模糊的实现

这部分参照《Unity Shader入门精要》实现。

首先根据书上所述创建一个用于屏幕后处理效果的基类`PostEffectsBase.cs`，检查各种资源和条件是否满足，并制定一个Shader来创建一个用于处理渲染纹理的材质。

然后创建`MotionBlur.cs`脚本继承`PostEffectsBase`基类，并绑定在场景的摄像头上。同时创建一个用于控制摄像头运动的脚本`Translating.cs`，并且也绑定在摄像头上。

在`Translating.cs`中，我们让摄像机做水平往复运动：

```c#
void Update () 
{
    transform.Translate(Time.deltaTime * speed, 0, 0, Space.Self);

    if (transform.position.x > startX)
        speed = -Speed;
    else if (transform.position.x < endX)
        speed = Speed;
}
```

在`MotionBlur.cs`中，首先声明运动模糊效果所使用的Shader，并创建相应的材质，同时定义模糊参数

```c#
public Shader motionBlurShader;
private Material motionBlurMaterial = null;

public Material material
{
	get
	{
		motionBlurMaterial = CheckShaderAndCreateMaterial(motionBlurShader, motionBlurMaterial);
        return motionBlurMaterial;
    }
}

[Range(0.0f, 0.9f)]
public float blurAmount = 0.5f;
```

然后定义一个用于保存之前图像叠加结果的`RenderTexture`类型的变量`accumulationTexture`，最后定义运动模糊所使用的`OnRenderImage`函数。在函数中，如果Texture满足条件，就把当前帧的图像和变量`accumulationTexture`中保存的图像混合，并将参数传递给材质，然后再更新`accumulationTexture`变量中的图像，并显示结果。代码如下：

```c#
void OnRenderImage(RenderTexture src, RenderTexture dest)
{
    if (material != null)
    {
        if (accumulationTexture == null || accumulationTexture.width != src.width || accumulationTexture.height != src.height)
        {
            DestroyImmediate(accumulationTexture);
            accumulationTexture = new RenderTexture(src.width, src.height, 0);
            accumulationTexture.hideFlags = HideFlags.HideAndDontSave;
            Graphics.Blit(src, accumulationTexture);
        }
        
        accumulationTexture.MarkRestoreExpected();

        material.SetFloat("_BlurAmount", 1.0f - blurAmount);

        Graphics.Blit(src, accumulationTexture, material);
        Graphics.Blit(accumulationTexture, dest);
    }
    else
    {
        Graphics.Blit(src, dest);
    }
}
```

然后在Shader中，需要定义渲染纹理和混合系数。顶点着色器中计算`FragmentData`的position和uv，然后定义两个片元着色器，一个用于更新RGB通道，一个用于更新α通道。

```c#
fixed4 fragRGB(FragmentData i) : SV_Target
{
	return fixed4(tex2D(_MainTex, i.uv).rgb, _BlurAmount);
}

half4 fragA(FragmentData i) : SV_Target
{
	return tex2D(_MainTex, i.uv);
}
```

最后定义运动模糊所需要的Pass，并且关闭Fallback。完成之后将Shader添加到`MotionBlur.cs`脚本的`motionBlurShader`参数中，运行便能够看到运动模糊的效果。

![](./pictures/MotionBlur.jpg)

### tips

为了方便展示效果，在摄像头运动上加入了控制的代码，当敲击空格时摄像头开始运动，再次敲击空格时摄像头暂停运动。