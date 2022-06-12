using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Linq;
using System.Text;

namespace Search
{
    public class NotifyHelper : INotifyPropertyChanged
    {
        private bool cab;
        private bool loop;
        private bool ipuLoop;
        private bool next;
        private bool prev;
        private bool pomps;
        private bool sildz;
        private bool viomps;
        private Point posYlblTC3;
        private Point posYlblTC4;
        private Point posYlblS2;
        public bool Cab
        {
            get { return cab; }
            set
            {
                cab = value;
                InvokePropertyChanged(new PropertyChangedEventArgs("Cab"));
            }
        }

        public bool Loop
        {
            get { return loop; }
            set
            {
                loop = value;
                InvokePropertyChanged(new PropertyChangedEventArgs("Loop"));
            }
        }

        public bool IpuLoop
        {
            get { return ipuLoop; }
            set
            {
                ipuLoop = value;
                InvokePropertyChanged(new PropertyChangedEventArgs("IpuLoop"));
            }
        }

        public bool Next
        {
            get { return next; }
            set
            {
                next = value;
                InvokePropertyChanged(new PropertyChangedEventArgs("Next"));
            }
        }

        public bool Prev
        {
            get { return prev; }
            set
            {
                prev = value;
                InvokePropertyChanged(new PropertyChangedEventArgs("Prev"));
            }
        }

        public bool Pomps
        {
            get { return pomps; }
            set
            {
                pomps = value;
                InvokePropertyChanged(new PropertyChangedEventArgs("Pomps"));
            }
        }

        public bool Sildz
        {
            get { return sildz; }
            set
            {
                sildz = value;
                InvokePropertyChanged(new PropertyChangedEventArgs("Sildz"));
            }
        }

        public bool Viomps
        {
            get { return viomps; }
            set
            {
                viomps = value;
                InvokePropertyChanged(new PropertyChangedEventArgs("Viomps"));
            }
        }

        public Point PosYlblTC3
        {
            get { return posYlblTC3; }
            set
            {
                posYlblTC3 = value;
                InvokePropertyChanged(new PropertyChangedEventArgs("PosYlblTC3"));
            }
        }

        public Point PosYlblTC4
        {
            get { return posYlblTC4; }
            set
            {
                posYlblTC4 = value;
                InvokePropertyChanged(new PropertyChangedEventArgs("PosYlblTC4"));
            }
        }

        public Point PosYlblS2
        {
            get { return posYlblS2; }
            set
            {
                posYlblS2 = value;
                InvokePropertyChanged(new PropertyChangedEventArgs("PosYlblS2"));
            }
        }

        #region Implementation of INotifyPropertyChanged

        public event PropertyChangedEventHandler PropertyChanged;

        public void InvokePropertyChanged(PropertyChangedEventArgs e)
        {
            PropertyChangedEventHandler handler = PropertyChanged;
            if (handler != null) handler(this, e);
        }

        #endregion
    }
}
