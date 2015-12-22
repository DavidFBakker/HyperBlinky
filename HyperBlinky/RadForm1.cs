using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Xml.Serialization;
using Ionic.Utils;
using LEDBlinkyControls;

namespace HyperBlinky
{
    public partial class RadForm1 : Form
    {
        private ConfigItem _configItem;
        public RadForm1()
        {
            _configItem = ConfigItems.GetConfig();
            InitializeComponent();
                      
            Load += RadForm1_Load;
        }

        private void RadForm1_Load(object sender, EventArgs e)
        {
            Setup();
        }

        private void Setup()
        {
            textBox1.Text = _configItem.LedBlinkyTemplateFile;
            textBox2.Text = _configItem.LEDBlinkyInputMap;
            textBox3.Text = _configItem.LEDBlinkyControls;
            textBox4.Text = _configItem.KeyMappingFile;
            textBox5.Text = _configItem.InputLabelAliases;
        }

        private void textBox1_TextChanged(object sender, EventArgs e)
        {
            _configItem.LedBlinkyTemplateFile = textBox1.Text;
        }

        private void textBox2_TextChanged(object sender, EventArgs e)
        {
            _configItem.LEDBlinkyInputMap = textBox2.Text;
            Util.LoadInputMap();
        }

        private void textBox3_TextChanged(object sender, EventArgs e)
        {
            _configItem.LEDBlinkyControls = textBox3.Text;
            Util.LoadControllerMap();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            var dlg1 = new FolderBrowserDialogEx
            {
                Description = "Select file:",
                ShowNewFolderButton = false,
                ShowEditBox = true,
                NewStyle = true,
                ShowBothFilesAndFolders = true,
                
                SelectedPath = _configItem?.LedBlinkyTemplateFile,
                ShowFullPathInEditBox = true,
                RootFolder = Environment.SpecialFolder.MyComputer
            };

            var result = dlg1.ShowDialog();
            if (result == DialogResult.OK)
            {
                textBox1.Text = dlg1.SelectedPath;
                _configItem.LedBlinkyTemplateFile = dlg1.SelectedPath;
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            var dlg1 = new FolderBrowserDialogEx
            {
                Description = "Select file:",
                ShowNewFolderButton = false,
                ShowEditBox = true,
                NewStyle = true,
                ShowBothFilesAndFolders = true,
                SelectedPath = _configItem?.LEDBlinkyInputMap,
                ShowFullPathInEditBox = true,
                RootFolder = Environment.SpecialFolder.MyComputer
            };

            var result = dlg1.ShowDialog();
            if (result == DialogResult.OK)
            {
                textBox2.Text = dlg1.SelectedPath;
                _configItem.LEDBlinkyInputMap = dlg1.SelectedPath;
            }

        }

        private void button3_Click(object sender, EventArgs e)
        {
            var dlg1 = new FolderBrowserDialogEx
            {
                Description = "Select file:",
                ShowNewFolderButton = false,
                ShowEditBox = true,
                ShowBothFilesAndFolders = true,
                NewStyle = true,
                SelectedPath = _configItem?.LEDBlinkyControls,
                ShowFullPathInEditBox = true,
                RootFolder = Environment.SpecialFolder.MyComputer
            };

            var result = dlg1.ShowDialog();
            if (result == DialogResult.OK)
            {
                textBox3.Text = dlg1.SelectedPath;
                _configItem.LEDBlinkyControls = dlg1.SelectedPath;
            }
        }

        private void button4_Click(object sender, EventArgs e)
        {
           var template = Util.ReadControllerMap();
            foreach (var item in template.Items)
            {
                datEmulator datEmulator = item as datEmulator;
                if (datEmulator != null)
                {
                    foreach (var controlGroup in datEmulator.controlGroup)
                    {
                        foreach (var controlGroupplayer in controlGroup.player.Where(a=>a.control != null))
                        {
                            foreach (var control in controlGroupplayer.control)
                            {
                                var ic = control.name.ToBlinkyInputCode();
                                if (!string.IsNullOrEmpty(ic))
                                    control.inputCodes = ic;
                            }
                        }
                    }
                    continue;
                }

                datFrontEnd datFrontEnd = item as datFrontEnd;
                if (datFrontEnd != null)
                {
                    foreach (var controlGroup in datFrontEnd.controlGroup)
                    {
                        foreach (var controlGroupplayer in controlGroup.player.Where(a => a.control != null))
                        {
                            foreach (var control in controlGroupplayer.control)
                            {
                                var ic = control.name.ToBlinkyInputCode();
                                if (!string.IsNullOrEmpty(ic))
                                    control.inputCodes = ic;
                            }
                        }
                    }
                    continue;
                }

                datControlDefaults datControlDefaults = item as datControlDefaults;
                if (datControlDefaults != null)
                {
                    foreach (var control in datControlDefaults.control)
                    {
                     
                                var ic = control.name.ToBlinkyInputCode();
                        if (!string.IsNullOrEmpty(ic))
                            control.inputCodes = ic;
                           
                    }
                    continue;
                }

            }

            var ser = new XmlSerializer(typeof(LEDBlinkyControls.dat));
            TextWriter writer = new StreamWriter(_configItem.LedBlinkyTemplateFile);
            ser.Serialize(writer, template);
            writer.Close();

        }

        private void textBox4_TextChanged(object sender, EventArgs e)
        {
            _configItem.KeyMappingFile = textBox4.Text;
        }

        private void button5_Click(object sender, EventArgs e)
        {
            var dlg1 = new FolderBrowserDialogEx
            {
                Description = "Select file:",
                ShowNewFolderButton = false,
                ShowEditBox = true,
                ShowBothFilesAndFolders = true,
                NewStyle = true,
                SelectedPath = _configItem?.KeyMappingFile,
                ShowFullPathInEditBox = true,
                RootFolder = Environment.SpecialFolder.MyComputer
            };

            var result = dlg1.ShowDialog();
            if (result == DialogResult.OK)
            {
                textBox4.Text = dlg1.SelectedPath;
                _configItem.KeyMappingFile=dlg1.SelectedPath;
            }
        }


        //From template
        private void button6_Click(object sender, EventArgs e)
        {
            var template = Util.ReadControllerMap(_configItem.LedBlinkyTemplateFile);
            foreach (var item in template.Items)
            {
                datEmulator datEmulator = item as datEmulator;
                if (datEmulator != null)
                {
                    foreach (var controlGroup in datEmulator.controlGroup)
                    {
                        foreach (var controlGroupplayer in controlGroup.player.Where(a => a.control != null))
                        {
                            foreach (var control in controlGroupplayer.control)
                            {
                                var ic = control.inputCodes.ToInputCode();
                                //if (!string.IsNullOrEmpty(ic))
                                    control.inputCodes = ic;
                            }
                        }
                    }
                    continue;
                }

                datFrontEnd datFrontEnd = item as datFrontEnd;
                if (datFrontEnd != null)
                {
                    foreach (var controlGroup in datFrontEnd.controlGroup)
                    {
                        foreach (var controlGroupplayer in controlGroup.player.Where(a => a.control != null))
                        {
                            foreach (var control in controlGroupplayer.control)
                            {
                                var ic = control.inputCodes.ToInputCode();
                                //if (!string.IsNullOrEmpty(ic))
                                    control.inputCodes = ic;
                            }
                        }
                    }
                    continue;
                }

                datControlDefaults datControlDefaults = item as datControlDefaults;
                if (datControlDefaults != null)
                {
                    foreach (var control in datControlDefaults.control)
                    {

                      var ic = control.inputCodes.ToInputCode();
                        //if (!string.IsNullOrEmpty(ic))
                            control.inputCodes = ic;

                    }
                    continue;
                }

            }

            var ser = new XmlSerializer(typeof(LEDBlinkyControls.dat));
            TextWriter writer = new StreamWriter(_configItem.LEDBlinkyControls);
            ser.Serialize(writer, template);
            writer.Close();
        }

        private void textBox5_TextChanged(object sender, EventArgs e)
        {
            _configItem.InputLabelAliases = textBox5.Text;
        }

        private void button7_Click(object sender, EventArgs e)
        {
            var dlg1 = new FolderBrowserDialogEx
            {
                Description = "Select file:",
                ShowNewFolderButton = false,
                ShowEditBox = true,
                NewStyle = true,
                ShowBothFilesAndFolders = true,
                SelectedPath = _configItem?.InputLabelAliases,
                ShowFullPathInEditBox = true,
                RootFolder = Environment.SpecialFolder.MyComputer
            };

            var result = dlg1.ShowDialog();
            if (result == DialogResult.OK)
            {
                textBox5.Text = dlg1.SelectedPath;
                _configItem.InputLabelAliases = dlg1.SelectedPath;
            }
        }
    }
}
