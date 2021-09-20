# DianaGIF



## **功能：**

1. 对于帧率较高的GIF图，修改其Delay值，并抽帧，保证整体播放速度不变

2. 等比例缩放GIF图片尺寸（仅支持等比例，不支持变形拉伸）

    

## 使用的第三方库

- [Magick.NET.SystemDrawing (4.0.5)](https://github.com/dlemstra/Magick.NET)
- [Magick.NET-Q8-AnyCPU (8.3.0)](https://github.com/dlemstra/Magick.NET)



## 界面外观

![image-20210920135959733](https://ssmiler.oss-cn-guangzhou.aliyuncs.com/img/image-20210920135959733.png)



## 关于GIF的播放速度

GIF的播放速度，取决于每一帧的`delay`值。delay值越小，播放速度越快。

[Frame Delay Times for Animated GIFs](https://www.deviantart.com/humpy77/journal/Frame-Delay-Times-for-Animated-GIFs-240992090)

> Process each graphic in the Data Stream in sequence, without delays other than those specified in the control information.

> Delay Time - If not 0, this field specifies the number of hundredths (1/100) of a second to wait before continuing with the processing of the Data Stream. The clock starts ticking immediately after the graphic is rendered.

就是说，如果

`DelayTime = 0`，则渲染完一帧就马上播放下一帧（当然这是不实际的）
`DelayTime = 1`，则0.01秒播放一帧，即100fps；
`DelayTime = 2`，则0.02秒播放一帧，即50fps；
`DelayTime = 3`，则0.03秒播放一帧，即33.3fps；
`DelayTime = 10`，则0.1秒播放一帧，即10fps；
……

DelayTime = 0是不实际的，如果cpu处理太快的话，gif图一瞬间就播放完了，完全没有动画的效果。

但是，通常我们的窗口并不会刷新这么快，所以即便 `delay=0` ，实际的播放速度看上去也不会很快。

- 对于一些播放器，当delay值过小的时候，会自动调整成固定值。比如某个播放器能接受的最小delay为3，那么当它播放delay=0,1,2的gif时，就会强制以delay=3的速度播放。
- 某些聊天软件的gif播放器就比较拉胯，无视图片本身携带的delay值，直接将播放速度固定成delay=6，对于delay=2的图，它以delay=6的速度播放，造成图片播放变慢；对于delay=10的图片，它也以delay=6的速度播放，造成图片播放速度变快。

