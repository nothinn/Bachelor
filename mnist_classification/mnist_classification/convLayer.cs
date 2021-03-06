﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace mnist_classification
{
    class ConvLayer
    {
        public int InputWidth { get; set; }

        public int InputHeight { get; set; }

        public int InputDepth { get; set; }

        public int OutputWidth { get; set; }

        public int OutputHeight { get; set; }

        public int OutputDepth { get; set; }

        public int Filters { get; set; }

        public int FilterSize { get; set; }

        public int Stride { get; set; }

        public int Pad { get; set; }

        public Filter[] FilterArray { get; set; }

        public float[] Bias { get; set; }


        //Input and output is given as width times height times depth.
        public float[,,] Output { get; set; }

        public float[,,] Input { get; set; }






        public void CalcConv()
        {

            Output = new float[OutputWidth, OutputHeight, OutputDepth];

            //Part of the array, used for convolution.
            float[,,] part = new float[FilterSize, FilterSize, InputDepth];


            for (int i = 0; i < InputWidth; i += Stride)
            {
                for (int j = 0; j < InputHeight; j += Stride)
                {
                    //Fill the part of the array that is to be calculated.
                    FillPart(part, i - Pad, j - Pad);


                    int depth = 0;
                    //Multiply each filter onto the array.
                    foreach (Filter filter in FilterArray)
                    {

                        //Max for ReLu function.
                        Output[i, j, depth] =  Max(MultFilter(filter, part) + Bias[depth],0);


                        depth++;
                    }
                }
            }
        }

        private float Max(float a, float b)
        {
            if (a > b) return a;
            return b;
        }

        private float MultFilter(Filter filter, float[,,] part)
        {
            float sum = 0;

            for(int i = 0; i< filter.Depth; i++)
            {
                for(int j = 0; j < filter.Height; j++)
                {
                    for(int k = 0; k< filter.Width; k++)
                    {
                        sum += filter.Weights[k, j, i] * part[k,j,i];
                    }
                }
            }
            return sum;
        }

        private void FillPart(float[,,] part, int i, int j)
        {
            //Fill temp array with values.
            for (int n = 0; n < FilterSize; n++)
            {
                for(int m = 0; m <FilterSize; m++)
                {
                    for (int k = 0; k < InputDepth; k++)
                    {
                        float temp = 0;

                        if (n + i >= InputWidth) {
                            temp = 0;
                        }
                        else if (m + j >= InputHeight)
                        {
                            temp = 0;
                        }
                        else if (n + i < 0 || m + j < 0) { temp = 0; }
                        else
                        {
                            temp = Input[n + i, m + j, k];
                        }


                        part[n, m, k] = temp;


                    }
                }
            }
        }
    }

}
