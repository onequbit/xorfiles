// C# 9.0

using System;
using System.IO;
using System.Collections.Generic;

Program.Require(args.Length == 2, "two arguments required");    
string f1 = args[0];
string f2 = args[1];
string outfile = "outfile_" + Program.TimeStamp() + ".out";

Program.Require(File.Exists(f1), $"{f1} not found");
Program.Require(File.Exists(f2), $"{f1} not found");

BinaryReader reader1 = new BinaryReader(File.Open(f1, FileMode.Open, FileAccess.Read, FileShare.ReadWrite));
BinaryReader reader2 = new BinaryReader(File.Open(f2, FileMode.Open, FileAccess.Read, FileShare.ReadWrite));
BinaryWriter writer = new BinaryWriter(File.Open(outfile, FileMode.Create));

long size1 = Program.GetFileSize(f1);
long size2 = Program.GetFileSize(f2);
long position = 0;

while (position < size1 && position < size2)
{
    byte b1 = reader1.ReadByte();
    byte b2 = reader2.ReadByte();
    byte result = (byte)(b1 ^ b2);
    writer.Write(result);
    position++;
}

int padding = Math.Abs( (int)(size1 - size2) );
BinaryReader slack = (size1 > size2) ? reader1 : reader2;
for (int i=0; i<padding; i++)
{    
    writer.Write(slack.ReadByte());
    position++;
}

reader1.Close();
reader2.Close();
writer.Close();
Program.Quit();

public class Program
{

    public static string TimeStamp()
    {
        string ts = DateTime.Now.ToString("s");
        ts = ts.Replace("-","").Replace("T","_").Replace(":","");
        return ts;
    }

    public static void Require(bool condition, string failMessage = "")
    {
        if (!condition)
        {
            failMessage.Print();
            Quit();
        }
    }

    public static long GetFileSize(string filename)
    {
        long size = new FileInfo(filename).Length;
        return size;
    }

    public static void Quit()
    {
        Environment.Exit(0);
    }    
}

public static partial class ExtensionMethods
{
    public static void Print(this string message)
    {
        Console.WriteLine(message);
    }

    public static bool Confirm(this string message)
    {        
        string response = "";
        while (response != "yes" && response != "no")
        {
            Console.Write($"{message} [yes/no]...");
            response = Console.ReadLine();
        }
        return response == "yes";        
    }
}


