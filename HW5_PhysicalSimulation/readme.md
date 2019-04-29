# HW5 实验报告

516030910381 姜凡叙

### 单摆模拟

##### 场景搭建

新建一个场景，在场景中放入四个球，一个作为顶点Top，其余三个分别为用三种方法实现的单摆球，并放置在同一位置。创建一个Empty Object用来搭载导出数据的脚本。

##### 单摆球属性

- 重力加速度 `g`
- 摆长 `l`
- 与竖直方向的夹角 `theta`
- 角速度 `omega`

##### 单摆球的运动

在`Update`中使用了如下代码来使小球进行摆动，并且计算与竖直方向的夹角：

```c#
Sphere.RotateAround(Top.position, new Vector3(0, 0, -1), transAngle);
theta = Vector3.Angle(Sphere.position - Top.position, new Vector3(0, -1, 0));
```

其中`transAngle`根据`omega`计算而来。

而根据公式$\omega^{n+1}=\omega ^n-\frac{g}{l}sin\theta \Delta t$

我们能够得到`deltaOmega`

```c#
float deltaOmega = g / l * Mathf.Sin(Mathf.Deg2Rad * theta) * Time.deltaTime;
```

然后能够得知新的`omega`

```c#
if (Top.position.x > Sphere.position.x)
{
    omega = omega - deltaOmega;
}
else
{
    omega = omega + deltaOmega;
}
```

在不同的方法中，只需要用不同的方式来计算`transAngle`就可以了。

##### Euler方法

根据公式$x(t_0+h)=x_0+hf(x_0)$

Euler方法直接使用`omega`得到`transAngle`

```c#
transAngle = omega;
```

##### Midpoint方法

根据公式$x(t_0+h)=x_0+hf(x_0+\frac{h}{2}f(x_0))$

Midpoint方法计算一半路程时的速度来计算`transAngle`

我们有公式$v_t^2-v_0^2=2ax$

在运动过程中近似认为`deltaT`内加速度恒定，则可以推测出运动到一半路程时的角速度为

$\omega_{half} = \sqrt{\omega_0^2+\omega_t^2}$

然后得到`transAngle`

```c#
if (omega > 0)
{
    transAngle = Mathf.Sqrt((Mathf.Pow(omega, 2) + Mathf.Pow(tmpOmega, 2)) / 2);
}
else
{
    transAngle = -Mathf.Sqrt((Mathf.Pow(omega, 2) + Mathf.Pow(tmpOmega, 2)) / 2);
}
```

##### Trapzoid方法

根据公式$x(t_0+h)=x_0+h\frac{f(x_0)+f(x_0+hf(x_0))}{2}$

Trapzoid方法计算前一个`omega`与后一个`omega`的平均值来计算`transAngle`

```c#
transAngle = (tmpOmega + omega) / 2;
```

##### 效果比较

分别将使用Euler方法、Midpoint方法和Trapzoid方法的小球涂成了红色、蓝色和黄色，当点击运行时能够观察到下面的结果：

![](./pictures/Pendulum.jpg)

可以看出使用Midpoint方法与Trapzoid方法的单摆球运动基本一致，而使用Euler方法的单摆球运动要滞后一些。随着时间的增长，三个球摆动的幅度都会越来越大，而使用Euler方法的单摆球幅度的变化更为快速、明显。

##### 数据输出与分析

这里使用`StreamWriter`来将运行的数据输出到文件当中，首先生成新的文件来储存数据，并初始化文件开头。

```c#
FileStream angleFile = new FileStream(
    @"D:/Proj/UnityProj/PhysicsAndHair/Data/Angle.txt", 
    FileMode.Create);
sw1 = new StreamWriter(angleFile);

FileStream energyFile = new FileStream(
    @"D:/Proj/UnityProj/PhysicsAndHair/Data/Energy.txt", 
    FileMode.Create);
sw2 = new StreamWriter(energyFile);

sw1.WriteLine("Time\tEuler\tMidpoint\tTrapzoid");
sw2.WriteLine("Time\tEuler\tMidpoint\tTrapzoid");
```

在`Update`得到每个小球当前与竖直方向的夹角和角速度，并且计算能量

```c#
theta1 = Vector3.Angle(EulerSphere.position - Top.position, 
                       new Vector3(0, -1, 0)) * Mathf.Deg2Rad;
theta2 = Vector3.Angle(MidpointSphere.position - Top.position, 
                       new Vector3(0, -1, 0)) * Mathf.Deg2Rad;
theta3 = Vector3.Angle(TrapzoidSphere.position - Top.position, 
                       new Vector3(0, -1, 0)) * Mathf.Deg2Rad;
theta1 = Top.position.x > EulerSphere.position.x ? -theta1 : theta1;
theta2 = Top.position.x > MidpointSphere.position.x ? -theta2 : theta2;
theta3 = Top.position.x > TrapzoidSphere.position.x ? -theta3 : theta3;

omega1 = EulerSphere.GetComponent<EulerController>().omega;
omega2 = MidpointSphere.GetComponent<MidpointController>().omega;
omega3 = TrapzoidSphere.GetComponent<TrapzoidController>().omega;
float energy1 = Mathf.Pow(omega1 * l, 2) / 2 + g * l * (1 - Mathf.Cos(theta1));
float energy2 = Mathf.Pow(omega2 * l, 2) / 2 + g * l * (1 - Mathf.Cos(theta2));
float energy3 = Mathf.Pow(omega3 * l, 2) / 2 + g * l * (1 - Mathf.Cos(theta3));
```

然后将结果输出到文件

```c#
float time = Time.time;
sw1.WriteLine(time.ToString("f2") + "\t" + 
              theta1.ToString("f2") + "\t" +
              theta2.ToString("f2") + "\t" + 
              theta3.ToString("f2"));

sw2.WriteLine(time.ToString("f2") + "\t" +
              energy1.ToString("f2") + "\t" +
              energy2.ToString("f2") + "\t" +
              energy3.ToString("f2"));
```

在Unity中运行场景一段时间，文件中便记录了许多数据，将这些数据导入到Excel当中，并且绘制图像，得到了三种方法下小球角度、能量随时间变化的图像

![](./pictures/Angle.jpg)

![](./pictures/Energy.jpg)

