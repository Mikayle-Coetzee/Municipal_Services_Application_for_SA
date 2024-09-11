using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;

namespace PROG7312_ST10023767.Classes
{
    public class ProgressBarManager
    {
        private ProgressBar progressBar;
        private int progressValue;

        public ProgressBarManager(ProgressBar progressBar)
        {
            this.progressBar = progressBar;
            this.progressValue = 0;
        }

        public void UpdateProgress(int value)
        {
            progressValue += value;
            progressBar.Value = progressValue;
        }

        public void ResetProgress()
        {
            progressValue = 0;
            progressBar.Value = 0;
        }

        public bool IsProgressComplete()
        {
            return progressValue == 100;
        }
    }
}
