using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using System.Windows.Forms;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;

namespace TextEditor
{
    class TextDocument
    {
        //used for the StreamReader
        String text = "";

        public TextDocument() { }

        public string FilePath
        { get; set; }

        //Grader example functions
        public Boolean Load(String fileName)
        {
            try
            {
                using (StreamReader sr = new StreamReader(fileName))
                {
                    text = "";
                    //is redeclaration here wrong?
                    String line = "";

                    // Read and display lines until the end of file is reached.
                    while ((line = sr.ReadLine()) != null)
                    {
                        text += line + "\n";
                    }
                }

                return true;
            }

            //catch other exceptions for StreamWriter?
            catch (Exception ex)
            {
                return false;
            }
        }
        public override String ToString()
        {
            return text;
        }

        public Boolean Save(String fileName, String text)
        {
            try
            {
                using (StreamWriter sw = new StreamWriter(fileName))
                {
                    String line = text;
                    sw.WriteLine(line);
                }

                return true;
            }

            //catch other exceptions for StreamWriter?
            catch (Exception ex)
            {
                return false;
            }
        }
    }
}
