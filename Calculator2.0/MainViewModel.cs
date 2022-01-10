using System;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Windows.Input;
using GalaSoft.MvvmLight.Command;

namespace Calculator2._0
{
    class MainViewModel : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;
        public void OnPropertyChanged([CallerMemberName] string prop = "")
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(prop));
        }

		private string _Count;
		private bool _isError;
		public ObservableCollection<string> history { get; set; }

		public ICommand Calculate { get; }
		public ICommand Erase { get; }
		public ICommand Delete { get; }
		public ICommand GetButtonValue { get; }


		private string _selectedText;

		public string SelectedText
		{
			get { return _selectedText; }
			set {
				_selectedText = value;
				OnPropertyChanged(nameof(SelectedText));
			}
		}


		public string Count
		{
			get => _Count;
			set
			{
				_Count = value;
				OnPropertyChanged(nameof(Count));
			}
		}

		public bool IsError
		{
			get { return _isError; }
			set
			{
				_isError = value;
				OnPropertyChanged(nameof(IsError));
			}
		}


		public MainViewModel()
		{
			history = new ObservableCollection<string>();

			Calc.ReadFromDB(history);

			Calculate = new RelayCommand<string>(x =>
			{
				string str = Count;

				if (history.Count < 10)
				{
					history.Insert(0, Count);
				}
				else
				{
					history.RemoveAt(9);
					history.Insert(0, Count);
				}

				Calc.SaveFile(Count);
				Calc.LoadToDB(Count);

				try
				{
					Count = Calc.Calculator(Count.Replace(',', '.'));
					str += "=" + Count;

				}
				catch
				{
					Count = "Ошибка в выражении";
					IsError = true;
				}
			});

			Erase = new RelayCommand<string>(x =>
			{
				if (IsError || String.IsNullOrEmpty(Count))
					return;
				Count = Count.Remove(Count.Length - 1);
			});


			Delete = new RelayCommand<string>(x =>
			{
				Count = "";
				IsError = false;

			});


			GetButtonValue = new RelayCommand<string>(x => 
			{
				Count += x;

			});


		}
		


	}
}
