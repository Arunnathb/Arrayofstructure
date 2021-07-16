using System;
using System.Collections.Generic;
using System.Text;
using System.Reflection;
using System.Runtime.InteropServices;
using log4net;
using System.Threading;
namespace newRTECreation
{
    class LogicInterface
    {
        private static readonly ILog logger = LogManager.GetLogger(typeof(LogicInterface));
        [StructLayout(LayoutKind.Sequential), Serializable]
        public struct InverterParam  // inverter structure measurements.
        {
            public InverterParam(double dummyvar)
            {
                InverterID = 0;
                NominalP = 0;
                NominalS = 0;
            }
            public double InverterID;
            public double NominalP;
            public double NominalS;
        }
        [StructLayout(LayoutKind.Sequential), Serializable]
        public class ExtU     //External input(root input signal with default storage)
        {
            public ExtU(double dummyvar)
                {
                  IDPS=new InverterParam[3];
        }
            [MarshalAs(UnmanagedType.ByValArray, SizeConst =3)]
            public InverterParam [] IDPS;
            public  int In_A;
           
        }
            [StructLayout(LayoutKind.Sequential), Serializable]             
        public static class ExtU_class    //input class
        {
            //  [MarshalAs(UnmanagedType.ByValArray, SizeConst = 10)]

            public static double[] IDPS1 = new double[9];
           // public static double[] IDPS=new double[9];
            public static  int In_A;
        }
        [StructLayout(LayoutKind.Sequential), Serializable]
        public struct ExtY     //External output
        {
            public int Int_In_A;                      /* '<Root>/Int_In_A' */
            public double TotalP;                          /* '<Root>/TotalP' */
            public double TotalS;                        /* '<Root>/TotalS' */

        }

    }
}
