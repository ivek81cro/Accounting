using AccountingUI.Core.Models;
using AccountingUI.Core.Services;
using BookUraModule.ModelsLocal;
using BookUraModule.ServiceLocal;
using Microsoft.Win32;
using Prism.Commands;
using Prism.Mvvm;
using Prism.Services.Dialogs;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Xml.Serialization;

namespace BookUraModule.Dialogs
{
    public class UraToXmlDialogViewModel : BindableBase, IDialogAware
    {
        private readonly IDataForXml _dataForXml;

        private sObrazacURA _ura = new();

        public UraToXmlDialogViewModel(IDataForXml dataForXml)
        {
            GenerateXmlCommand = new DelegateCommand(GenerateXml, CanGenerate);
            _dataForXml = dataForXml;
        }

        public DelegateCommand GenerateXmlCommand { get; private set; }

        public string Title => "XML za ePoreznu";

        public event Action<IDialogResult> RequestClose;

        private ObservableCollection<BookUraRestModel> _uraList;
        public ObservableCollection<BookUraRestModel> UraList
        {
            get { return _uraList; }
            set { SetProperty(ref _uraList, value); }
        }

        private string _autor;
        public string Autor
        {
            get { return _autor; }
            set 
            { 
                SetProperty(ref _autor, value);
                GenerateXmlCommand.RaiseCanExecuteChanged();
            }
        }

        private DateTime[] _period;
        public DateTime[] Period
        {
            get { return _period; }
            set { SetProperty(ref _period, value); }
        }

        private string _statusMessage;
        public string StatusMessage
        {
            get { return _statusMessage; }
            set { SetProperty(ref _statusMessage, value); }
        }

        public bool CanCloseDialog()
        {
            return true;
        }

        public void OnDialogClosed()
        {

        }

        public void OnDialogOpened(IDialogParameters parameters)
        {
            var list = parameters.GetValue<List<BookUraRestModel>>("collection");
            UraList = new ObservableCollection<BookUraRestModel>(list.Where(x=>x.UkupniPretporez != 0));
            Period = parameters.GetValue<DateTime[]>("period");
        }

        private bool CanGenerate()
        {
            return Autor != null && Autor.Split(' ').Count() > 1;
        }

        private async void GenerateXml()
        {
            _ura = await _dataForXml.GenerateXml(UraList.ToList(), Autor, Period);
            SaveToFile();
        }

        private void SaveToFile()
        {
            SaveFileDialog sfd = new SaveFileDialog()
            {
                Filter = "XML file|*.xml",
                Title = "Spremi XML datoteku"
            };
            Nullable<bool> result = sfd.ShowDialog();
            string path;
            if (result != null && result == true)
            {
                path = sfd.FileName;

                TextWriter txtWriter = new StreamWriter(path);
                XmlSerializer x = new XmlSerializer(_ura.GetType());
                x.Serialize(txtWriter, _ura);
                txtWriter.Close();

                StatusMessage = "Datoteka kreirana";
            }
        }
    }
}
