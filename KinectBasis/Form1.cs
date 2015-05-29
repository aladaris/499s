using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace KinectBasis {
    public partial class Form1 : Form {

        KinectHanlder _kinectHandler;

        public Form1() {
            InitializeComponent();
            _kinectHandler = new KinectHanlder();
        }

        private void Form1_Load(object sender, EventArgs e) {

        }
    }
}
