# DianaGIF

对gif图进行压缩。



## TODO

#### 提高健壮性

文件路径需要判断是否合法，能否找到文件，若是找不到，弹出警告窗。



# GIF

[Frame Delay Times for Animated GIFs](https://www.deviantart.com/humpy77/journal/Frame-Delay-Times-for-Animated-GIFs-240992090)

> Process each graphic in the Data Stream in sequence, without delays other than those specified in the control information.

> Delay Time - If not 0, this field specifies the number of hundredths (1/100) of a second to wait before continuing with the processing of the Data Stream. The clock starts ticking immediately after the graphic is rendered.

就是说，如果

`DelayTime = 0`，则渲染完一帧就马上播放下一帧（当然这是不实际的）？
`DelayTime = 1`，则0.01秒播放一帧，即100fps；
`DelayTime = 2`，则0.02秒播放一帧，即50fps；
`DelayTime = 3`，则0.03秒播放一帧，即33.3fps；
`DelayTime = 10`，则0.1秒播放一帧，即10fps；
……

因为DelayTime = 0是不实际的，如果cpu处理太快的话，gif图播放就在一瞬间，完全没有动画的效果，

>  Some programs, JASC Animation Shop for example, will not allow a 0 delay. As each frame in a GIF can have it's own local colour map, some programs have even used the 0 delay to create static GIFs with more that 256 colours[2].

