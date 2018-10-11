using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

using System.Windows.Forms;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;

namespace TextEditor
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
        }

        TextDocument textDocument = new TextDocument();
        String currentFilePath = "new.txt";
        Boolean modified = false, alreadyBeenSaved = false;

        private void exit_menuItem_Click(object sender, RoutedEventArgs e)
        {
            if (handleSaveRequest())
            {
                Close();
            }
        }

        private void open_menuItem_Click(object sender, RoutedEventArgs e)
        {
            if (handleSaveRequest())
            {
                startLoad();
            }
        }

        private void save_menuItem_Click(object sender, RoutedEventArgs e)
        {
            //if document has not previously been saved then it will go through saveDialogBox process
            if (alreadyBeenSaved == false)
            {
                startSave();
            }
            
            //Grader
            else
            {
                String text = notes_textBox.Text;
                if (!textDocument.Save(textDocument.FilePath, text))
                {
                    System.Windows.Forms.MessageBox.Show("An error occurred saving the document.", "Text Editor", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }

                unsetModifiedState();
            }
        }

        private void save_as_menuItem_Click(object sender, RoutedEventArgs e)
        {
            startSave();
        }

        private void startLoad()
        {
            OpenFileDialog openFileDialog = new OpenFileDialog();

            openFileDialog.Filter = "txt files (*.txt)|*.txt";

            //From example code. What is FitlerIndex doing exactly? Is 2 wrong
            openFileDialog.FilterIndex = 2;
            openFileDialog.RestoreDirectory = true;

            if (openFileDialog.ShowDialog() == System.Windows.Forms.DialogResult.OK)
            {
                textDocument.FilePath = openFileDialog.FileName;
                if (textDocument.Load(textDocument.FilePath))
                {
                    notes_textBox.Text = textDocument.ToString();

                    unsetModifiedState();
                    alreadyBeenSaved = true;
                }
                else
                {
                    System.Windows.Forms.MessageBox.Show("An error occurred loading the document.", "Text Editor", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }

        private void startSave()
        {
            SaveFileDialog saveFileDialog = new SaveFileDialog();

            currentFilePath = textDocument.FilePath;
            if (currentFilePath != "")
            {
                saveFileDialog.FileName = System.IO.Path.GetFileName(currentFilePath);
                saveFileDialog.InitialDirectory = System.IO.Path.GetDirectoryName(currentFilePath);
            }

            saveFileDialog.DefaultExt = ".txt";
            saveFileDialog.Filter = "txt files (*.txt)|*.txt";

            //From example code. What is FitlerIndex doing exactly? Is 2 wrong
            saveFileDialog.FilterIndex = 1;
            saveFileDialog.RestoreDirectory = true;

            if (saveFileDialog.ShowDialog() == System.Windows.Forms.DialogResult.OK)
            {
                String text = notes_textBox.Text;
                textDocument.FilePath = saveFileDialog.FileName;
                if (!textDocument.Save(textDocument.FilePath, text))
                {
                    System.Windows.Forms.MessageBox.Show("An error occurred saving the document.", "Text Editor", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }

                unsetModifiedState();
                alreadyBeenSaved = true;
            }
        }

        private Boolean handleSaveRequest()
        {
            if (!modified) return true;

            string messageBoxText = "This text file has not been saved.\nDo you want to save?";
            string caption = "Text Editor";
            MessageBoxButton button = MessageBoxButton.YesNoCancel;
            MessageBoxImage icon = MessageBoxImage.Warning;
            // Display message box
            MessageBoxResult messageBoxResult = System.Windows.MessageBox.Show(messageBoxText, caption, button, icon);
            // Process message box results
            switch (messageBoxResult)
            {
                case MessageBoxResult.Yes: // Save document and exit
                    startSave();
                    return true;

                case MessageBoxResult.No: // Exit without saving
                    return true;

                case MessageBoxResult.Cancel: // Don't exit
                    return false;
            }

            return false;
        }

        private void about_menuItem_Click(object sender, RoutedEventArgs e)
        {
            string messageBoxText = "Long Live Dale Musser";
            string caption = "About";
            MessageBoxButton button = MessageBoxButton.OK;
            MessageBoxImage icon = MessageBoxImage.Information;
            // Display message box
            System.Windows.MessageBox.Show(messageBoxText, caption, button, icon);
        }

        private void notes_textBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            setModifiedState();
        }
        private void setModifiedState()
        {
            modified = true;
            save_menuItem.IsEnabled = true;

        }
        private void unsetModifiedState()
        {
            modified = false;
            save_menuItem.IsEnabled = false;

        }
        private void new_menuItem_Click(object sender, RoutedEventArgs e)
        {
            if (handleSaveRequest())
            {
                startNewTextDocument();
            }
        }
        private void startNewTextDocument()
        {
            notes_textBox.Text = "";

            unsetModifiedState();
            if (currentFilePath != "")
            {
                textDocument.FilePath = System.IO.Path.GetDirectoryName(currentFilePath) + "/new.txt";
            }
        }
    }
}
