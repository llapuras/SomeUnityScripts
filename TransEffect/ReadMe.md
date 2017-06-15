AlphaTransEffect
--------------

Image透明度渐变脚本。
Gradient Transparent scripts for Image.

Variables:

WaitTimeBeforeFirstLoop————第一次循环动画开始之前的等待时间
FadeInTime————每张image用来渐入的时间
FadeOutTime————每张image用来渐出的时间
StillLifeTime————每张image保持静止显示的时间
ImageReloadTime————每次播放下一张imgae的间隔时间，从StillLifeTime结束后开始计算，正常情况下应满足ImageReloadTime+FadeInTime>=FadeOutTime，否则图片FadeOut不完全
LoopTimes————循环的次数
Transform lib————图片列表父transform
循环结束后最后一张图片简便消失并删除图片列表。
    
:joy:
