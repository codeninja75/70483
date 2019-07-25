using System;
using System.IO;
using System.IO.Compression;
using System.IO.IsolatedStorage;

namespace DotNet.E70483.ImplementDataAccess
{
    public class PreformIO
    {
        public void ExerciseStreams()
        {
            FileSystemInfo fsi;  //base class not createable

            if (!File.Exists("bob.txt")) { File.Create("bob.txt"); }
            //FileInfo
            FileInfo fi = new FileInfo("bob.txt");

            fi.Attributes = FileAttributes.Hidden | FileAttributes.Archive;

            Console.WriteLine(fi.Attributes.ToString());

            //copy a file

            if (File.Exists("bob.txt.bak")) { File.Delete("bob.txt.bak"); }

            fi.CopyTo("bob.txt.bak");

            //Directory Info
            DirectoryInfo di = new DirectoryInfo(".");
            Console.WriteLine(di.Attributes);
            Console.WriteLine(di.FullName);
            //Enum files in a dir
            FileInfo[] allFiles = di.GetFiles("*.txt");
            for (int i = 0; i < allFiles.Length; i++)
            {
                Console.WriteLine(allFiles[i].Name);
            }
            //Drive info
            DriveInfo dri = new DriveInfo("c");
            Console.WriteLine("Avilalble free space on c:" + dri.AvailableFreeSpace.ToString());
            Console.WriteLine("Format of c:" + dri.DriveFormat);
            //drive type enum
            Console.WriteLine("drive type of c:" + dri.DriveType.ToString());
            //how to enum drives
            DriveInfo[] diArr = DriveInfo.GetDrives();
            for (int i = 0; i < diArr.Length; i++)
            {
                Console.WriteLine("Drive {0} is type {1}", diArr[i].Name, diArr[i].DriveType.ToString());
            }
            //OR
            foreach (DriveInfo drive in diArr)
            {
                Console.WriteLine("Drive {0} is type {1}", drive.Name, drive.DriveType.ToString());
            }
            //Path
            Console.WriteLine(Path.GetExtension(fi.FullName));
            Console.WriteLine(Path.GetTempFileName());
            //how to change file extension
            Console.WriteLine("Change Path: {0}", Path.ChangeExtension(fi.FullName, "spam"));
            //FileSystemWatcher
            FileSystemWatcher fsw = new FileSystemWatcher(@"c:\projects");
            //How to monitor a directory
            fsw.Changed += new FileSystemEventHandler(fsw_Changed);
            fsw.Deleted += new FileSystemEventHandler(fsw_Deleted);
            fsw.Renamed += new RenamedEventHandler(fsw_Renamed);
            fsw.Created += new FileSystemEventHandler(fsw_Created);
            fsw.Error += new ErrorEventHandler(fsw_Error);
            fsw.EnableRaisingEvents = true;
            Console.ReadLine();



            #region Lesson 2
            //Reading & writing files
            Stream s; //Abstract class, cannot directly create

            //streams

            //file + directory classes & the streams they return
            //file classes can open files
            StreamWriter sw = new StreamWriter(fi.OpenWrite());
            sw.WriteLine("Here's some good ole fashioned text!");
            sw.Close();

            StreamReader sr = fi.OpenText();

            Console.WriteLine(sr.ReadToEnd());
            sr.Close();
            FileInfo[] fiArry = di.GetFiles("*.txt");
            //fiArry[0].OpenWrite

            //FileAccess enum
            //FileAccess.Read 
            //FileAccess.ReadWrite 
            //FileAccess.Write 

            //filemode enum
            //Append , Create , CreateNew, Open, OpenOrCreate, Truncate
            //filestream  read&write as bytes
            FileStream fs = new FileStream(fi.FullName, FileMode.Open);
            //can only read bytes...

            //streamreader
            //see above
            //HT read from a file

            StreamReader rdr = File.OpenText(@"C:\boot.ini");
            while (!rdr.EndOfStream)
            {
                string line = rdr.ReadLine();
                if (line.Contains("boot"))
                {
                    Console.WriteLine("Found boot:");
                    Console.WriteLine(line);
                    break;
                }
            }
            rdr.Close();



            //StreamWriter
            //HT write to a file
            //see above

            //Memory stream
            MemoryStream ms = new MemoryStream();
            StreamWriter msw = new StreamWriter(ms);
            msw.WriteLine("Hello my honey!");
            msw.WriteLine("Hello my baby!");
            msw.WriteLine("Hello my ragtime GAL!");
            msw.Flush();
            FileStream mfs = new FileStream("memstream.txt", FileMode.OpenOrCreate);
            ms.WriteTo(mfs);
            msw.Close();
            ms.Close();
            mfs.Close();

            //Buffered stream
            FileStream newfile = File.Create("Buffed.txt");
            BufferedStream bs = new BufferedStream(newfile);

            StreamWriter bsw = new StreamWriter(bs);
            bsw.WriteLine("this is buffered!");
            bsw.Close();
            bs.Close();
            newfile.Close();


            #endregion

            #region Lession 3
            //HT compress data
            FileStream sourcefile = File.OpenRead("buffed.txt");
            FileStream destfile = File.Create("buffed.zip");
            GZipStream compstream = new GZipStream(destfile, CompressionMode.Compress);
            int theByte = sourcefile.ReadByte();
            while (theByte != -1)
            {
                compstream.WriteByte((byte)theByte);
                theByte = sourcefile.ReadByte();
            }
            sourcefile.Close();
            compstream.Close();
            destfile.Close();
            //HT decompress data
            sourcefile = File.OpenRead("buffed.zip");
            destfile = File.Create("buffed.unzip.txt");
            compstream = new GZipStream(sourcefile, CompressionMode.Decompress);
            theByte = compstream.ReadByte();
            while (theByte != -1)
            {
                destfile.WriteByte((byte)theByte);
                theByte = compstream.ReadByte();
            }
            sourcefile.Close();
            destfile.Close();
            compstream.Close();
            StreamReader dsr = new StreamReader(File.OpenRead("buffed.unzip.txt"));
            Console.WriteLine(dsr.ReadToEnd());
            //need to open the uncompressed file and read it out!
            #endregion

            #region Lession 4
            //Isolated storage!!!
            //IsolatedStorgaeFileClass
            //assembly/machine store = application data
            //assembly/user: user level
            //HT create a store
            IsolatedStorageFile Machine = IsolatedStorageFile.GetMachineStoreForAssembly();  //this dll only!

            //Machine = IsolatedStorageFile.GetMachineStoreForApplication(); // the calling application
            Machine = IsolatedStorageFile.GetMachineStoreForDomain(); //for the app domain, which can span exe's & dlls
            IsolatedStorageFile user = IsolatedStorageFile.GetUserStoreForAssembly(); //this dll
            //user = IsolatedStorageFile.GetUserStoreForApplication();// the calling app
            user = IsolatedStorageFile.GetUserStoreForDomain(); //for the app domain

            //IsolatedStorageFilestream class
            IsolatedStorageFileStream userStream = new IsolatedStorageFileStream("UserSettings.dat", FileMode.OpenOrCreate, user);
            IsolatedStorageFileStream machineStream = new IsolatedStorageFileStream("MachineSettings.dat", FileMode.OpenOrCreate, Machine);
            //Reading & Writing data to a IS
            StreamWriter userW = new StreamWriter(userStream);
            userW.WriteLine("This is user data!");
            userW.Close();
            StreamWriter machineW = new StreamWriter(machineStream);
            machineW.WriteLine("machine data!");
            machineW.Close();
            userStream = new IsolatedStorageFileStream("UserSettings.dat", FileMode.OpenOrCreate, user);
            machineStream = new IsolatedStorageFileStream("MachineSettings.dat", FileMode.OpenOrCreate, Machine);
            StreamReader r = new StreamReader(userStream);
            Console.WriteLine(r.ReadToEnd());
            r.Close();
            r = new StreamReader(machineStream);
            Console.WriteLine(r.ReadToEnd());

            //directories

            string[] MDir = Machine.GetDirectoryNames("SomeDir");
            if (MDir.Length == 0)
            {
                Machine.CreateDirectory("SomeDir");
            }


            //ISFilePermission class : see top of class
            //Permitting Isolated storage
            #endregion
        }

        private void fsw_Error(object sender, ErrorEventArgs e)
        {
            throw new NotImplementedException();
        }

        private void fsw_Created(object sender, FileSystemEventArgs e)
        {
            throw new NotImplementedException();
        }

        private void fsw_Renamed(object sender, RenamedEventArgs e)
        {
            throw new NotImplementedException();
        }

        private void fsw_Deleted(object sender, FileSystemEventArgs e)
        {
            throw new NotImplementedException();
        }

        private void fsw_Changed(object sender, FileSystemEventArgs e)
        {
            throw new NotImplementedException();
        }
    }
}
