﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;
using Beginning.Commands;

namespace Beginning.ViewModels
{
    public class MainWindowViewModel : INotifyPropertyChanged
    {
        private readonly ICommand _startCommand;
        private int _currentProgress;
        private string _calculationFinished;

        public MainWindowViewModel()
        {
            _startCommand = new CustomCommand(StartCommandCanExecute, StartCommandExecute);
            CalculateCommand = new AsyncCommand(ExecuteSubmitAsync, CanExecuteSubmit);
        }

        private bool CanExecuteSubmit() => true;

        private async Task ExecuteSubmitAsync()
        {
            await CalculateDifficultalgorithm();
            CalcualtionFinished = "Finished calculation";
        }

        public event PropertyChangedEventHandler PropertyChanged;

        public ICommand StartCommand { get { return _startCommand; } }

        public IAsyncCommand CalculateCommand { get; set; }

        public int CurrentProgress
        {
            get => _currentProgress;
            set
            {
                _currentProgress = value;
                OnPropertyChanged(nameof(CurrentProgress));
            }
        }

        public string CalcualtionFinished
        {
            get => _calculationFinished;
            set
            {
                _calculationFinished = value;
                OnPropertyChanged(nameof(CalcualtionFinished));
            }
        }

        private void StartCommandExecute(object parameter)
        {

            _currentProgress = 0;

            BackgroundWorker worker = new BackgroundWorker();
            worker.WorkerReportsProgress = true;
            worker.DoWork += Worker_DoWork;
            worker.ProgressChanged += Worker_ProgressChanged;
            //worker.RunWorkerCompleted += worker_RunWorkerCompleted;
            worker.RunWorkerAsync(10000);
        }

        private bool StartCommandCanExecute(object parameter)
        {
            return true;
        }

        public void OnPropertyChanged([CallerMemberName] string prop = "")
        {
            if (PropertyChanged != null)
                PropertyChanged(this, new PropertyChangedEventArgs(prop));
        }

        void Worker_DoWork(object sender, DoWorkEventArgs e)
        {
            int max = 10;
            int result = 0;
            for (int i = 0; i < max; i++)
            {
                int progressPercentage = Convert.ToInt32(((double)i / max) * 100);
                (sender as BackgroundWorker).ReportProgress(progressPercentage, i);
                System.Threading.Thread.Sleep(1000);
                result++;
            }
            e.Result = result;
        }

        void Worker_ProgressChanged(object sender, ProgressChangedEventArgs e)
        {
            _currentProgress = e.ProgressPercentage;
            OnPropertyChanged(nameof(CurrentProgress));
        }

        private async Task CalculateDifficultalgorithm()
        {
            CalculationService();
        }

        private void CalculationService()
        {
            Task.Delay(4000).Wait();
        }
    }
}
