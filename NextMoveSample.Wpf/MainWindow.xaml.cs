using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Security.Cryptography.X509Certificates;
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
using Microsoft.Win32;
using NextMove.Lib;

namespace NextMoveSample.Wpf
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private NextMoveClient nextMoveClient;
        public ObservableCollection<Process> Processes { get; set; }
        public ObservableCollection<Status> Statuses { get; set; }
        public ObservableCollection<Message> SentMessages { get; set; }
        public ObservableCollection<Message> ReceivedMessages { get; set; }
        public ObservableCollection<FileInfo> SelectedFiles { get; set; }
        public const string StoragePath = @"C:\temp\efmormidling\mottak\";

    
        public Status SelectedStatus { get; set; }
        public string SelectedProcessId { get; set; }
        public int SelectedSecurityLevel { get; set; }
        public string SenderId { get; set; }
        public string RecevierId { get; set; }

        public MainWindow()
        {
            InitializeComponent();
            
            this.DataContext = this;
            SenderId = "910075918";
            RecevierId = "910075918";
            InitProcesses();
            GetMessages();
            InitStatuses();
            InitSentAndReceivedMessages();
            InitSelectedFiles();
            
        }

        private async Task GetMessages()
        {
            //var messages = await nextMoveClient.GetAllMessages();
            var mess = await nextMoveClient.GetMessage(MessageTypes.DPO, new DirectoryInfo(StoragePath));

            if (mess != null)
            {
                ReceivedMessages.Add(new Message(mess));
            }
        }

        private async Task InitProcesses()
        {
             Processes = new ObservableCollection<Process>();
            nextMoveClient = new NextMoveClient(new HttpClient());
            var result = await nextMoveClient.GetCapabilities(RecevierId);
            foreach (var capability in result.CapabilitiesList)
            {
                if (capability.ServiceIdentifier == "DPO")
                {
                    Processes.Add(new Process(capability.Process, capability.PrettyProcessName));
                }
            }


            //Processes = new ObservableCollection<Process>()
            //{
            //    new Process( @"urn:no:difi:profile:arkivmelding:planByggOgGeodata:ver1.0", "Plan bygg og geodata"),
            //    new Process( @"urn:no:difi:profile:arkivmelding:helseSosialOgOmsorg:ver1.0", "Helse sosial og omsorg"),
            //    new Process( @"urn:no:difi:profile:arkivmelding:oppvekstOgUtdanning:ver1.0", "Oppvekst og utdanning")

            //};
        }

        private void InitStatuses()
        {
            Statuses = new ObservableCollection<Status>()
            {
                new Status
                {
                    ConversationId = Guid.NewGuid().ToString(),
                    MessageId = Guid.NewGuid().ToString(),
                    StatusName = "Status1"
                },
                new Status
                {
                    ConversationId = Guid.NewGuid().ToString(),
                    MessageId = Guid.NewGuid().ToString(),
                    StatusName = "Status2"
                }
            };
        }

        private void InitSelectedFiles()
        {
            SelectedFiles = new ObservableCollection<FileInfo>();
        }

        private void InitSentAndReceivedMessages()
        {
            SentMessages = new ObservableCollection<Message>()
            {
                new Message()
                {
                    ConversationId = Guid.NewGuid().ToString(), MessageId = Guid.NewGuid().ToString(),
                    Receiver = new Organization {Id = "123456789", Name = "Org1"},
                    Sender = new Organization {Id = "321654987", Name = "Me"}
                },
                new Message()
                {
                    ConversationId = Guid.NewGuid().ToString(), MessageId = Guid.NewGuid().ToString(),
                    Receiver = new Organization {Id = "987654321", Name = "Org2"},
                    Sender = new Organization {Id = "321654987", Name = "Me"}
                }
            };
            
            ReceivedMessages = new ObservableCollection<Message>()
            {
                new Message()
                {
                    ConversationId = Guid.NewGuid().ToString(), MessageId = Guid.NewGuid().ToString(),
                    Sender = new Organization {Id = "123456789", Name = "Org1"},
                    Receiver = new Organization {Id = "321654987", Name = "Me"}
                },
                new Message()
                {
                    ConversationId = Guid.NewGuid().ToString(), MessageId = Guid.NewGuid().ToString(),
                    Sender = new Organization {Id = "987654321", Name = "Org2"},
                    Receiver = new Organization {Id = "321654987", Name = "Me"}
                }
            };
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            
            var openFileDialog = new OpenFileDialog {Multiselect = true};

            if (openFileDialog.ShowDialog() != true) return;
            
            foreach (var fileName in openFileDialog.FileNames)
            {
                SelectedFiles.Add(new FileInfo(fileName));
            }
            
        }

        private void UIElement_OnDragEnter(object sender, DragEventArgs e)
        {
            e.Effects = DragDropEffects.Copy;
        }

        private void UIElement_OnDrop(object sender, DragEventArgs e)
        {
            if (!e.Data.GetDataPresent(DataFormats.FileDrop)) return;
            // Note that you can have more than one file.
               
            string[] files = (string[])e.Data.GetData(DataFormats.FileDrop);

            // Assuming you have one file that you care about, pass it off to whatever
            // handling code you have defined.
            foreach (var file in files)
            {
                SelectedFiles.Add(new FileInfo(file));
            }
            //var f = new FileInfo(files[0]);
                
            //SelectedFiles.Add(f);
        }


        private async void SendButton_OnClick(object sender, RoutedEventArgs e)
        {
            var envelope = new EnvelopeInfo(SenderId, RecevierId, "urn:no:difi:profile:arkivmelding:helseSosialOgOmsorg:ver1.0", "urn:no:difi:arkivmelding:xsd::arkivmelding")
            {
                ConversationId = Guid.NewGuid().ToString(),
                MessageId = Guid.NewGuid().ToString()
            };
            var businessMessage = new DpoBusinessMessage
            {
                PrimaryDocumentName = "arkivmelding.xml", SecurityLevel = SelectedSecurityLevel
            };
            var result = await nextMoveClient.SendMessage(envelope, businessMessage, SelectedFiles);

        }

        private void ResetButton_OnClick(object sender, RoutedEventArgs e)
        {
           SelectedFiles.Clear();
        }
    }

    public class Status
    {
        public string MessageId { get; set; }
        public string ConversationId { get; set; }
        public string StatusName { get; set; }

    }

    public class Message
    {
        public Organization Sender { get; set; }
        public Organization Receiver { get; set; }
        public string ConversationId { get; set; }
        public string MessageId { get; set; }

        public Message()
        {
            
        }

        public Message(StandardBusinessDocument standardBusinessDocument)
        {
            Sender =  new Organization{Id = standardBusinessDocument.StandardBusinessDocumentHeader.Sender[0].Identifier.Value};
            Receiver =  new Organization{Id = standardBusinessDocument.StandardBusinessDocumentHeader.Receiver[0].Identifier.Value};
            ConversationId = standardBusinessDocument.StandardBusinessDocumentHeader.BusinessScope.Scope.FirstOrDefault(s => s.Type == "ConversationId").InstanceIdentifier;
            MessageId = standardBusinessDocument.StandardBusinessDocumentHeader.DocumentIdentification.InstanceIdentifier;
        }

        public void ViewPayload(string path)
        {
            System.Diagnostics.Process.Start("explorer.exe", $@"{path}\{MessageId}");
        }

        public int Sikkerhetsnivå { get; set; }

        public string FilePath { get; set; }


    }

    public class Organization
    {
        public string Id { get; set; }
        public string Name { get; set; }

        
    }

    public class Process
    {
        public Process(string id, string name)
        {
            Id = id;
            Name = name;
        }

        public string Name { get; set; }
        public string Id { get; set; }
    }

    
}
