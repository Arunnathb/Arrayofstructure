using log4net;
using log4net.Config;
using System;
using System.IO;
using System.Collections.Generic;
using System.Reflection;
using System.Runtime.InteropServices;
using System.Threading;

namespace newRTECreation
{

    class Program
    {
        private static readonly ILog logger = LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);
        private NativeLibrary astyle = null;
      

        [DllImport("InvParamEx", CharSet = CharSet.Unicode)]
        public static extern void InvParamEx_initialize();

        [DllImport("InvParamEx", CharSet = CharSet.Unicode)]
        public static extern void InvParamEx_step();

        [DllImport("InvParamEx", CharSet = CharSet.Unicode)]
        public static extern void InvParamEx_terminate();

        static void Main(string[] args)
        {

            // Console.WriteLine("Hello World!");

            Program PPC = new Program();

            PPC.RunApplication();

        }
        void RunApplication()
        {
            //   Console.WriteLine("Entered Inside Run FUnction");
            IntPtr hdl = IntPtr.Zero;
            // int hdl = 0;
            if (OSinterface.IsWindows())
            {

                // logger.Info("Identified OS is Windows");
                astyle = (NativeLibrary)new NativeLibraryWindows();
                try
                {
                    hdl = astyle.LoadLibrary("InvParamEx.dll");
                    //hdl = astyle.LoadLibrary("LogX_RF_win64.dll");
                    //  hdl = astyle.LoadLibrary(@"E:\\R&D TEam\\RTE\\NewDevelopment\\newRTECreation\\newRTECreation\\lib\\LogX_RF.dll");

                    if (hdl != IntPtr.Zero)
                    {
                        Console.WriteLine("The Dll has been Loaded Sucessfully");
                    }

                    // Console.WriteLine(hdl);
                }
                catch (Exception e)
                {
                    Console.WriteLine("PPC_APPL DLL Load error or not found: Exiting the application" + e.Message);
                    Environment.Exit(1);
                }
            }
            else
            {
                Console.WriteLine("Identified OS is Linux");
                NativeLibraryLinux astyle = new NativeLibraryLinux();
                try
                {
                    //hdl = astyle.LoadLibrary(appC.PPCSODir + "/PPC_MVP01.so");
                    hdl = astyle.LoadLibrary("./InvParamEx.so");
                    Console.WriteLine("This is an Linux OS");
                }
                catch (Exception e)
                {
                    Console.WriteLine("PPC_APPL.SO Load error or not found: Exiting the application" + e.Message);
                }

            }

            IntPtr addr_rtU = (IntPtr)astyle.GetProcAddress(hdl, "rtU");
            IntPtr addr_rtY = (IntPtr)astyle.GetProcAddress(hdl, "rtY");
            inputtextfunction();

            if (addr_rtU != IntPtr.Zero)
            {
                // logger.Info("Initializing the PPC Model");

                Console.WriteLine("Initializing the Logic");
                try
                {
                    InvParamEx_initialize();
                }
                catch (Exception ex)
                {
                    // logger.Error("Error in initializg the PPC Model" + ex.Message);
                    Console.WriteLine("Error in Initializong the logic" + ex.Message);
                }

            }

            else
            {
                // logger.Error("Input structure pointer is Zero. Please check if PPC SO file exists. Exiting the application. ");
                Console.WriteLine("Input Structure pointer is Zero.Please check the dll file exists.");
                // Environment.Exit(1);
            }
            setInputs(addr_rtU);
            try
            {
                InvParamEx_step();
            }
            catch (Exception ex)
            {
                logger.Error("Error Running Model Step, output will not be generated: " + ex.Message);
                Console.WriteLine("Error File");
                // continue;
            }

            if (addr_rtU != IntPtr.Zero)
            {
                // Processing Output
                // logger.Info("Processing Output");
                processOutput(addr_rtY, addr_rtU);
            }
            else
            {
                // logger.Error("No output was generated from the model");
                Console.WriteLine("No output was generated from the model");

            }

            astyle.FreeLibrary(hdl);
            Console.WriteLine("Terminating the Model");
          //  InvParamEx_terminate();

        }
        static void inputtextfunction()
        { 

            var FileUrl = "inputdata.txt";

            string[] testdata = File.ReadAllLines(FileUrl);
            Console.WriteLine(testdata);
           

            foreach (string str in testdata)
            {


                Console.WriteLine("Arraay structure checking");
                Console.WriteLine(str);
                if (string.IsNullOrEmpty(str) == false)
                {
                    String[] attributes = str.Split(',');

                    foreach (var attribute in attributes)
                    {

                           var keyvalues = attribute.Split();

                        if (keyvalues[0].Equals("IDPS"))
                        {
                            Console.WriteLine(keyvalues);
                            int k = 0;

                            for (int l=0; l<9; l++)
                            {
                                k += 1;
                                LogicInterface.ExtU_class.IDPS1[l] = Convert.ToDouble(keyvalues[k]);
                                  
                               
                            }
                           
                        }
                        else if (keyvalues[0].Equals("In_A"))
                        {
                            LogicInterface.ExtU_class.In_A = Convert.ToInt32(keyvalues[1]);
                           // Console.WriteLine("IN_A=" + keyvalues[1]);
                        }
                    }
                }

            }
        }

        static void setInputs(IntPtr addr_rtU)
        {
            LogicInterface.ExtU receivedMeas = new LogicInterface.ExtU(0);
            {
                Console.WriteLine("Input received from external file");
                receivedMeas.In_A = LogicInterface.ExtU_class.In_A;
                  
                   int s = 2;
                   int j = 4;
                  //int q = 6;
                  // int z = 8;
                
               for(int ires=0;ires<3;ires++)   // struct size   test
                {
                    int l = 0;
                    int m = 1;
                    int n = 2;
                    if (ires == 0)
                    {
                        receivedMeas.IDPS[ires].InverterID = LogicInterface.ExtU_class.IDPS1[l + ires];
                        receivedMeas.IDPS[ires].NominalP = LogicInterface.ExtU_class.IDPS1[m + ires];
                        receivedMeas.IDPS[ires].NominalS = LogicInterface.ExtU_class.IDPS1[n + ires];
                     //   Console.WriteLine("Inside the array");
                        Console.WriteLine(receivedMeas.IDPS[ires].InverterID);
                        Console.WriteLine(receivedMeas.IDPS[ires].NominalP);
                        Console.WriteLine(receivedMeas.IDPS[ires].NominalS);
                    }
                    if(ires==1)
                    {
                        receivedMeas.IDPS[ires].InverterID = LogicInterface.ExtU_class.IDPS1[l + ires+s];
                        receivedMeas.IDPS[ires].NominalP = LogicInterface.ExtU_class.IDPS1[m + ires+s];
                        receivedMeas.IDPS[ires].NominalS = LogicInterface.ExtU_class.IDPS1[n + ires+s];
                     //   Console.WriteLine("Inside the array");
                        Console.WriteLine(receivedMeas.IDPS[ires].InverterID);
                        Console.WriteLine(receivedMeas.IDPS[ires].NominalP);
                        Console.WriteLine(receivedMeas.IDPS[ires].NominalS);
                    }
                    if(ires==2)
                    {
                        receivedMeas.IDPS[ires].InverterID = LogicInterface.ExtU_class.IDPS1[l + ires+j];
                        receivedMeas.IDPS[ires].NominalP = LogicInterface.ExtU_class.IDPS1[m + ires+j];
                        receivedMeas.IDPS[ires].NominalS = LogicInterface.ExtU_class.IDPS1[n + ires+j];
                      //  Console.WriteLine("Inside the array");
                        Console.WriteLine(receivedMeas.IDPS[ires].InverterID);
                        Console.WriteLine(receivedMeas.IDPS[ires].NominalP);
                        Console.WriteLine(receivedMeas.IDPS[ires].NominalS);
                    }

                }
              
            }

             Marshal.StructureToPtr(receivedMeas, addr_rtU, false);
             Console.WriteLine("In_A=" + receivedMeas.In_A);

        }

        static private void processOutput(IntPtr addr_rtY, IntPtr addr_rtU)
        {
            var output = (LogicInterface.ExtY)Marshal.PtrToStructure(addr_rtY, typeof(LogicInterface.ExtY));
            Console.WriteLine("Output Received from .Dll");
            Console.WriteLine("Int_In_A=" + output.Int_In_A);
            Console.WriteLine("TotalP=" + output.TotalP);
            Console.WriteLine("TotalS=" + output.TotalS);

        }
    }
}


