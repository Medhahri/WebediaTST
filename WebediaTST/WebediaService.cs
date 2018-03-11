using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Linq;
using System.ServiceProcess;
using System.Text;
using System.Threading.Tasks;
using System.Configuration;
using System.Runtime.InteropServices;
using WebediaTST.Models.ViewModels;
using System.IO;
using WebediaTST.Models.Repositories;
using System.Web;
using System.Web.Http;
using System.Web.Http.SelfHost;


namespace WebediaTST
{
    public partial class WebediaService : ServiceBase
    {
    
        [DllImport("advapi32.dll", SetLastError = true)]
        private static extern bool SetServiceStatus(IntPtr handle, ref ServiceStatus serviceStatus);

        private static readonly log4net.ILog log = log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        private IEventRepository eventRepository;
        private int eventId = 1;

        public WebediaService()
        {
            InitializeComponent();
            eventRepository = new EventRepository();

            System.Timers.Timer t = new System.Timers.Timer();
            t.AutoReset = false;
            t.Elapsed += new System.Timers.ElapsedEventHandler(this.OnTimer);
            t.Interval = 60000;
            t.Start();

        }

        public void OnTimer(object sender, System.Timers.ElapsedEventArgs args)
        {
            // TODO: Insert monitoring activities here.  
            log.Info(string.Format("Monitoring the System {0} {1}", EventLogEntryType.Information, eventId++));

            var path = ConfigurationManager.AppSettings["PathFolder"];
            CheckFolder(path);
        }  

        protected override void OnStart(string[] args)
        {
            #region Update User State
            // Update the service state to Start Pending.  
            ServiceStatus serviceStatus = new ServiceStatus();
            serviceStatus.dwCurrentState = ServiceState.SERVICE_START_PENDING;
            serviceStatus.dwWaitHint = 100000;
            SetServiceStatus(this.ServiceHandle, ref serviceStatus);

            // Update the service state to Running.  
            serviceStatus.dwCurrentState = ServiceState.SERVICE_RUNNING;
            SetServiceStatus(this.ServiceHandle, ref serviceStatus);
            #endregion

            #region Web Api Config
            // Web API routes
            var config = new HttpSelfHostConfiguration("http://localhost:8080");

            config.Routes.MapHttpRoute(
                name: "Api",
                routeTemplate: "api/{controller}/{id}",
                defaults: new { id = RouteParameter.Optional }
            );
            #endregion

            log.Info("In OnStart");
        }

        protected override void OnStop()
        {
            log.Info("In onStop.");
        }

        public void CheckFolder(string path)
        {
            // If a directory is not specified, exit program.
            if (path.Length != 1)
            {
                // Display the proper way to call the program.
                log.Fatal("Error : Folder path not found.");
                return;
            }
            try
            {
                // Create a new FileSystemWatcher and set its properties.
                FileSystemWatcher watcher = new FileSystemWatcher();
                watcher.Path = path;

                // Watch both files and subdirectories.
                watcher.IncludeSubdirectories = true;

                // Watch for all changes specified in the NotifyFilters
                //enumeration.
                watcher.NotifyFilter = NotifyFilters.Attributes |
                NotifyFilters.CreationTime |
                NotifyFilters.DirectoryName |
                NotifyFilters.FileName;

                // Watch all files.
                watcher.Filter = "*.*";

                // Add event handlers.
                watcher.Changed += new FileSystemEventHandler(OnChanged);
                watcher.Created += new FileSystemEventHandler(OnChanged);

                //Start monitoring.
                watcher.EnableRaisingEvents = true;

            }
            catch (IOException e)
            {
                log.Error("A Exception Occurred :" + e);
            }

            catch (Exception oe)
            {
                log.Error("An Exception Occurred :" + oe);
            }
        }

        // Define the event handlers.
        public void OnChanged(object source, FileSystemEventArgs e)
        {
            // Specify what is done when a file is changed.
            log.Info(string.Format("{0}, with path {1} has been {2}", e.Name, e.FullPath, e.ChangeType));

            EventModel eventFile = new EventModel()
            {
                EventType = e.ChangeType.ToString(),
                Path = e.FullPath,
                Date = new DateTime()
            };

            eventRepository.Add(eventFile);

            log.Info("Event is successfully saved.");
        }

    }
}
