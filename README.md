# PixelateImg

## Overview

This project solves the problem of outputting an image and then griding and 
resampling and coloring according to the specified resolution.
The color mode is to select the most color as the color of each grid 
with a specified resolution based on the color weight of the 
pixels in the grid.

The grid is divided from the center of the image to ensure the symmetry of the upper, lower and left. Input a picture, and finally output two pictures, one color picture, one black and white picture.

## Test

`Input`

![](https://pic4rain.oss-cn-beijing.aliyuncs.com/img/figma.jpg)

`output1` `resolution ratio 25`

![](https://pic4rain.oss-cn-beijing.aliyuncs.com/img/figma2.jpg)

`output2`  `resolution ratio 25`

![](https://pic4rain.oss-cn-beijing.aliyuncs.com/img/figma3.jpg)

`output3`  `resolution ratio 10`

![](https://pic4rain.oss-cn-beijing.aliyuncs.com/img/figma4.jpg)

`output4`  `resolution ratio 10`

![](https://pic4rain.oss-cn-beijing.aliyuncs.com/img/figma5.jpg)