using ESCPOS_NET.Emitters;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.IO.Ports;
using System.Linq;
using System.Runtime.InteropServices;
using Microsoft.Extensions.Logging;
using System.Threading;
using System.Threading.Tasks;
using ESCPOS_NET.ConsoleTest.test_files;
using ESCPOS_NET.Emitters.BaseCommandValues;
using ESCPOS_NET.Utilities;

namespace ESCPOS_NET.ConsoleTest
{
    internal class Program
    {
        private static BasePrinter printer;
        private static ICommandEmitter e;

        static async Task Main(string[] args)
        {

            Console.WriteLine("Welcome to the ESCPOS_NET Test Application!");
            Console.Write("Would you like to see all debug messages? (y/n): ");
            var response = Console.ReadLine().Trim().ToLowerInvariant();
            var logLevel = LogLevel.Information;
            if (response.Length >= 1 && response[0] == 'y')
            {
                Console.WriteLine("Debugging enabled!");
                logLevel = LogLevel.Trace;
            }

            var factory = LoggerFactory.Create(b => b.AddConsole().SetMinimumLevel(logLevel));
            var logger = factory.CreateLogger<Program>();
            ESCPOS_NET.Logging.Logger = logger;

            Console.WriteLine("1 ) Test Serial Port");
            Console.WriteLine("2 ) Test Network Printer");
            Console.WriteLine("3 ) Test Samba-Shared Printer");
            Console.Write("Choice: ");
            string comPort = "";
            string ip;
            string networkPort;
            string smbPath;
            response = Console.ReadLine();
            var valid = new List<string> { "1", "2", "3" };
            if (!valid.Contains(response))
            {
                response = "1";
            }

            int choice = int.Parse(response);

            if (choice == 1)
            {
                if (RuntimeInformation.IsOSPlatform(OSPlatform.Windows))
                {
                    while (!comPort.StartsWith("COM"))
                    {
                        Console.Write("COM Port (enter for default COM5): ");
                        comPort = Console.ReadLine();
                        if (string.IsNullOrWhiteSpace(comPort))
                        {
                            comPort = "COM15";
                        }
                    }

                    Console.Write("Baud Rate (enter for default 115200): ");
                    if (!int.TryParse(Console.ReadLine(), out var baudRate))
                    {
                        baudRate = 115200;
                    }

                    printer = new SerialPrinter(portName: comPort, baudRate: baudRate);
                    
                }
                else if (RuntimeInformation.IsOSPlatform(OSPlatform.Linux))
                {
                    Console.Write("File / virtual com path (eg. /dev/usb/lp0): ");
                    comPort = Console.ReadLine();
                    if (string.IsNullOrWhiteSpace(comPort))
                    {
                        comPort = "/dev/usb/lp0";
                    }

                    printer = new FilePrinter(filePath: comPort, false);
                }
            }
            else if (choice == 2)
            {
                Console.Write("IP Address (eg. 192.168.1.240): ");
                ip = Console.ReadLine();
                if (string.IsNullOrWhiteSpace(ip))
                {
                    ip = "192.168.254.202";
                }

                Console.Write("TCP Port (enter for default 9100): ");
                networkPort = Console.ReadLine();
                if (string.IsNullOrWhiteSpace(networkPort))
                {
                    networkPort = "9100";
                }

                printer = new NetworkPrinter(settings: new NetworkPrinterSettings()
                    { ConnectionString = $"{ip}:{networkPort}" });
            }
            else if (choice == 3)
            {
                Console.Write(@"SMB Share Name (eg. \\computer\printer): ");
                smbPath = Console.ReadLine();

                printer = new SambaPrinter(tempFileBasePath: @"C:\Temp", filePath: smbPath);
            }

            bool monitor = false;
            Thread.Sleep(500);
            if (choice != 3) // SMB printers do not support reads so status back will not work.
            {
                Console.Write("Turn on Live Status Back Monitoring? (y/n): ");
                response = Console.ReadLine().Trim().ToLowerInvariant();
                if (response.Length >= 1 && response[0] == 'y')
                {
                    monitor = true;
                }
            }
            //Результаты
            
            // byte[] official = new[]
            // {
            //     1c 3c 45 50 4f 53 3e 18 1b 2a 74
            //     32 30 34 52 1b 26 6c 32 30 35 50
            //     1b 2a 62 32 4d 1b 2a 70 31 36 59
            //     1b 2a 70 30 58 1b 2a 62 31
            // };
            // /////////////////////////////////
            //
            // //test #4
            // 1c 3c 45 50 4f 53 3e 18 1b 2a 74
            // 32 30 34 52 1b 26 6c 32 30 35 50
            // 1b 2a 62 32 4d 1b 2a 70 31 36 59
            // 1b 2a 70 30 58 1b 2a 62 31 
            //
            // 39 57
            // 00 ff ff 07 fe 00 ff 80 3f e0 00
            // 00 02 03 ff ff 07 f8 00 1b  
            //
            //
            //
            //
            // //test #3
            // 1c 3c 45 50 4f 53 3e 18 1b 2a 74
            // 32 30 34 52 1b 26 6c 32 30 35 50
            // 1b 2a 62 32 4d 1b 2a 70 31 36 59
            // 1b 2a 70 30 58 1b 2a 62 31
            //
            // 37 57
            // 00 ff ff 07 fe 00 00 04 1f ff f8
            // 03 ff ff 07 f8 00 1b 2a 62 
            //
            // fe 00 ff ff 03 e0 00 00 02 03 fe
            // 00 00 03 3f ff ff 02 f8 00 1b 2a
            // 62 30 4d 1b 2a 72 42
            //     
            // // test #2
            // 1c 3c 45 50 4f 53 3e 18 1b 2a 74
            // 32 30 34 52 1b 26 6c 32 31 34 50
            // 1b 2a 62 32 4d 1b 2a 70 31 36 59
            // 1b 2a 70 30 58 1b 2a 62 32 
            //
            // 35 57
            // 00 ff ff 05 c0 ff f0 3f 03 f0 3f
            // ff f0 00 03 ff ff 02 00 0f 
            //
            // 3f 03 f0 00 ff ff 06 00 00 02 1b
            // 2a 62 32 36 57 00 fc 0f ff fc 0f
            // c0 ff f0 3f fc 0f ff 00 00 02 3f
            // 03 f0 00 ff ff 06 00 00 02 1b 2a
            // 62 32 36 57 00 fc 0f ff fc 0f c0
            //     ff f0 3f fc 0f ff 00 00 02  
            //     
            // // test #1
            // 1c 3c 45 50 4f 53 3e 18 1b 2a 74
            // 32 30 34 52 1b 26 6c 32 31 34 50
            // 1b 2a 62 32 4d 1b 2a 70 31 36 59
            // 1b 2a 70 30 58 1b 2a 62 32 35 57
            // 00 ff ff 05 c0 fc 00 3f 03 f0 3f
            // ff f0 00 03 ff ff 02 00 0f  
            //     
            //     
            // 00 fc 0f ff fc 0f c0 ff ff 03 fc
            // 0f ff 00 00 02 3f 03 f0 00 ff ff
            // 06 00 00 02 1b 2a 62 32 36 57 00
            // fc 0f ff fc 0f c0 ff ff 03 fc 0f
            // ff 00 00 02 3f 03 f0 00 ff ff 06
            // 00 00 02 1b 2a 62 32 36 57  
                
                
                
    

            // var fontBident = 41;
            // var e = new EPSON();
            // // var str = "https://ya.ru";
            // // var strArray = str.ToCharArray().Select(x => (byte)x).ToArray();
            // // var strLen = str.Length + 3;
            // // byte b = (byte)(strLen % 256);
            // // byte b2 = (byte)(strLen / 256);
            // String content = "Hello !!";
            // byte[] content_bytes = content.ToCharArray().Select(x => (byte)x).ToArray();
            // int store_len = content_bytes.Length + 3;
            // byte store_pL = (byte) (store_len % 256);
            // byte store_pH = (byte) (store_len / 256);
            // byte[] FUNC_165 = new byte[] { 0x1b, 0x2a, 0x72, 0x30, 0x44, 0x1b, 0x2a, 0x72, 0x30, 0x45};
            // byte[] FUNC_167 = new byte[]
            // {
            //     0x1c, 0x3c, 0x45, 0x50, 0x4f, 0x53, 0x3e, 0x18, 0x1b, 0x2a, 0x74,
            //     0x32, 0x30, 0x34, 0x52, 0x1b, 0x26, 0x6c, 0x31, 0x39, 0x30, 0x50,
            //     0x1b, 0x2a, 0x62, 0x32, 0x4d, 0x1b, 0x2a, 0x70, 0x31, 0x36, 0x59,
            //     0x1b, 0x2a, 0x70, 0x30, 0x58, 0x1b, 0x2a, 0x62, 0x32, 0x32, 0x57,
            //     0x00, 0xff, 0xff, 0x05, 0xc0, 0x00, 0x0f, 0xc0, 0xfc, 0x00, 0x00,
            //     0x02, 0xfc, 0x00, 0x3f, 0xfc, 0x0f, 0xff, 0xff, 0x04
            // };  
            //
            // byte[] FUNC_169 = new byte[] { 0x1b, 0x2a, 0x72, 0x30, 0x64, 0x1b, 0x2a, 0x72, 0x30, 0x65 };
            //
            // printer.Write(FUNC_165);
            // printer.Write(FUNC_167);
            // printer.Write(FUNC_169);
           
                // printer.Write( // or, if using and immediate printer, use await printer.WriteAsync
            //     ByteSplicer.Combine(


                    // e.CenterAlign(),
                    // // e.PrintImage(File.ReadAllBytes("images/logo.png"), true),
                    // // e.PrintLine(""),
                    //
                    // new byte[]
                    // {
                    //     29,40,107,3,0,65,0
                    // },
                    // // new byte[]
                    // // {
                    // //     29,40,107,3,0,49,66,3
                    // // },
                    // new byte[]
                    // {
                    //     29,40,107,3,0,49,66,4
                    // },
                    // new byte[]
                    // {
                    //     29,40,107,3,0,49,69,0
                    // },
                    // new byte[]
                    // {
                    //     29,40,107,b,b2,49,80,49,strArray
                    // },
                    
                    // e.PrintQr()
            
            //     )
            // );
            // var qr = new QrTest();
            // while (true)
            // {
            //     printer.Write(qr.Print("https://ta.ru"));
            //     await Task.Delay(500);
            //     printer?.Write(e.FullCut());
            //     printer?.Write(e.EjectPaperAfterCut());
            //     await Task.Delay(500);
            // }
            // }
        e = new EPSON();
        var testCases = new Dictionary<Option, string>()
        {
            { Option.SingleLinePrinting, "Зашифрованные слова" },
            { Option.MultiLinePrinting, "Multi-line Printing" },
            { Option.LineSpacing, "Line Spacing" },
            { Option.BarcodeStyles, "Barcode Styles" },
            { Option.BarcodeTypes, "Barcode Types" },
            { Option.TwoDimensionCodes, "2D Codes" },
            { Option.TextStyles, "Text Styles" },
            { Option.FullReceipt, "Full Receipt" },
            { Option.CodePages, "Code Pages (Euro, Katakana, Etc)" },
            { Option.Images, "Images" },
            { Option.LegacyImages, "Legacy Images" },
            { Option.LargeByteArrays, "Large Byte Arrays" },
            { Option.CashDrawerPin2, "Cash Drawer Pin2" },
            { Option.CashDrawerPin5, "Cash Drawer Pin5" },
            { Option.Exit, "Exit" }
        
        };
        // printer.Write(e.Initialize());
        // printer.Write(e.Enable());
        // printer.Write(e.EnableAutomaticStatusBack());

        // printer.SerialPrinter.PaperStatus += delegate(object sender, byte[] bytes)
        // {
        //     if(bytes.Length == 0 ) return;
        //     if()
        // };
        //
        //
        //
        // printer.Write(e.PrintImage(QrGenerator.Generated("std"), false, true));
        // printer.Write(e.PrintImage(QrGenerator.Generated("std"), false, true));
        // printer.Write(e.PrintImage(QrGenerator.Generated("std"), false, true));
        // printer.Write(e.FullCut());
        // printer.Write(new byte[]{0x10,0x04,0x04});
        // // var i = printer.Status;
        // Task.Delay(10000);
        // int bytestoread = 0;
        //
        // Console.ReadLine();
        // printer.Write(e.PrintLine("421342134213424ddd"));
        // printer.Write(e.PrintLine("421342134213424ddd"));
        // printer.Write(e.PrintLine("421342134213424ddd"));
        // printer.Write(e.PrintLine("421342134213424ddd"));
        // printer.Write(e.PrintLine("421342134213424ddd"));
        // printer.Write(e.FullCut());
        // Task.Delay(10000);
        // printer.Write(new byte[]{0x10,0x04,0x04});
        // Console.ReadLine();
        
        
        
        
        // printer.StatusChanged += (sender, eventArgs) =>
        // {
        //     Console.ReadLine();
        // };
        // Console.ReadLine();

        // var s = printer.Port.ReadLine();
        

        // var totalBytes = printer.Reader.BaseStream.ReadBytes();
        // var totalChars = printer.Reader.BaseStream.BeginRead()
        
        // if (totalBytes >= 0 && totalBytes <= 255)
        // {
        //     printer.ReadBuffer.Enqueue((byte)totalBytes);
        //     printer.DataAvailable();
        // }
        //
        // byte[] buffer = new byte[1];
        // for (int len = 0; len < buffer.Length;)
        // {
        //     len += printer.Port.Read(buffer, len, buffer.Length - len);
        // }
        // var res = ByteArrayToString(buffer);  
        
        
        // var s = printer.Port.ReadByte();
        while (true)
        {
            foreach (var item in testCases)
            {
                Console.WriteLine($"{(int)item.Key} : {item.Value}");
            }
        
            Console.Write("Execute Test: ");
        
            if (!int.TryParse(Console.ReadLine(), out choice) || !Enum.IsDefined(typeof(Option), choice))
            {
                Console.WriteLine("Invalid entry. Please try again.");
                continue;
            }
        
            var enumChoice = (Option)choice;
            if (enumChoice == Option.Exit)
            {
                return;
            }
        
            Console.Clear();
        
            if (monitor)
            {
                printer.Write(e.Initialize());
                printer.Write(e.Enable());
                printer.Write(e.EnableAutomaticStatusBack());
            }
        
            Setup(monitor);
        
            printer?.Write(e.PrintLine($"== [ Start {testCases[enumChoice]} ] =="));
        
            switch (enumChoice)
            {
                case Option.SingleLinePrinting:
                    printer.Write(Tests.SingleLinePrinting(e));
                    break;
                case Option.MultiLinePrinting:
                    printer.Write(Tests.MultiLinePrinting(e));
                    break;
                case Option.LineSpacing:
                    printer.Write(Tests.LineSpacing(e));
                    break;
                case Option.BarcodeStyles:
                    printer.Write(Tests.BarcodeStyles(e));
                    break;
                case Option.BarcodeTypes:
                    printer.Write(Tests.BarcodeTypes(e));
                    break;
                case Option.TwoDimensionCodes:
                    printer.Write(Tests.TwoDimensionCodes(e));
                    break;
                case Option.TextStyles:
                    printer.Write(Tests.TextStyles(e));
                    break;
                case Option.FullReceipt:
                    printer.Write(Tests.Receipt(e));
                    break;
                case Option.Images:
                    printer.Write(Tests.Images(e, false));
                    break;
                case Option.LegacyImages:
                    printer.Write(Tests.Images(e, true));
                    break;
                case Option.LargeByteArrays:
                    try
                    {
                        printer.Write(Tests.TestLargeByteArrays(e));
                    }
                    catch (Exception e)
                    {
                        Console.WriteLine(
                            $"Aborting print due to test failure. Exception: {e?.Message}, Stack Trace: {e?.GetBaseException()?.StackTrace}");
                    }
        
                    break;
                case Option.CashDrawerPin2:
                    printer.Write(Tests.CashDrawerOpenPin2(e));
                    break;
                case Option.CashDrawerPin5:
                    printer.Write(Tests.CashDrawerOpenPin5(e));
                    break;
                default:
                    Console.WriteLine("Invalid entry.");
                    break;
            }
        
            Setup(monitor);
            printer?.Write(e.PrintLine($"== [ End {testCases[enumChoice]} ] =="));
            printer?.Write(e.PartialCutAfterFeed(5));
        
            // TODO: also make an automatic runner that runs all tests (command line).
        }
    }

    public enum Option
        {
            SingleLinePrinting = 1,
            MultiLinePrinting,
            LineSpacing,
            BarcodeStyles,
            BarcodeTypes,
            TwoDimensionCodes,
            TextStyles,
            FullReceipt,
            CodePages,
            Images,
            LegacyImages,
            LargeByteArrays,
            CashDrawerPin2,
            CashDrawerPin5,
            Exit = 99
        }

        private static void StatusChanged(object sender, EventArgs ps)
        {
            var status = (PrinterStatusEventArgs)ps;
            if (status == null) { Console.WriteLine("Status was null - unable to read status from printer."); return; }
            Console.WriteLine($"Printer Online Status: {status.IsPrinterOnline}");
            Console.WriteLine(JsonConvert.SerializeObject(status));
        }
        private static bool _hasEnabledStatusMonitoring = false;

        private static void Setup(bool enableStatusBackMonitoring)
        {
            if (printer != null)
            {
                // Only register status monitoring once.
                if (!_hasEnabledStatusMonitoring)
                {
                    printer.StatusChanged += StatusChanged;
                    _hasEnabledStatusMonitoring = true;
                }
                printer?.Write(e.Initialize());
                printer?.Write(e.Enable());
                if (enableStatusBackMonitoring)
                {
                    printer.Write(e.EnableAutomaticStatusBack());
                }
            }
        }
    }
}
