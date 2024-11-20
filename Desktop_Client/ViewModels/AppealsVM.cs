using CRM_Helper;
using CRM_APIRequests;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Input;

namespace Desktop_Client
{
    internal class AppealsVM : ViewModel
    {
        private readonly SkillProfiWebClient _spClient = new SkillProfiWebClient();

        public AppealsVM()
        {
            SetDateDiapasone = new LamdaCommand(OnSetDateDiapasone, CanAnyWay);
            EditConsultationStatus = new LamdaCommand(OnEditConsultationStatus, CanEdit);
            SaveConsultationStatus = new LamdaCommand(OnSaveConsultationStatus, CanAnyWay);

            IsOpenEditStatusMenuElement = false;

            UpdateConsultations();
            FirstDate = DateTime.Now;
            LastDate = DateTime.Now;

        }


        public void UpdateConsultations()
        {

            Consultations = new(Task.Run(async () =>
                await UserContext.SPClient.Consultations.GetListAsync()).Result);

            FilteredConsultations = new
                (ConsultationsFilter.FilterByDate(Consultations, LastDate, FirstDate));
        }


        private AppealStatus _selectedStatus;
        public AppealStatus SelectedStatus
        {
            get => _selectedStatus;
            set => Set(ref _selectedStatus, value);
        }

        private Appeal _selectedConsultation;
        public Appeal SelectedConsultation
        {
            get => _selectedConsultation;
            set
            {
                IsObjectSelected = value == null ? false : true;
                Set(ref _selectedConsultation, value);
            }
        }

        public ICommand SaveConsultationStatus { get; }
        private void OnSaveConsultationStatus(object p)
        {
            IsOpenEditStatusMenuElement = false;

            Task.Run(async () => await UserContext.SPClient.Consultations
                .EditAsync(SelectedConsultation.Id.ToString(), SelectedConsultation)).Wait();

            UpdateConsultations();

        }

        public bool CanEdit(object p) => IsObjectSelected;
        public ICommand EditConsultationStatus { get; }
        private void OnEditConsultationStatus(object p)
        {
            IsOpenEditStatusMenuElement = true;
        }

        private bool _isOpenEditStatusMenuElement;
        public bool IsOpenEditStatusMenuElement
        {
            get => _isOpenEditStatusMenuElement;
            set => Set(ref _isOpenEditStatusMenuElement, value);
        }

        public List<AppealStatus> ConsultationStatuses
        {
            get => Appeal.Statuses;
        }


        private DateTime _firstDate;
        public DateTime FirstDate
        {
            get => _firstDate;
            set
            {
                Set(ref _firstDate, value);
                FilteredConsultations = new
                    (ConsultationsFilter.FilterByDate(Consultations, LastDate, FirstDate));
            }
        }

        private DateTime _lastDate;
        public DateTime LastDate
        {
            get => _lastDate;
            set
            {
                Set(ref _lastDate, value);
                FilteredConsultations = new
                    (ConsultationsFilter.FilterByDate(Consultations, LastDate, FirstDate));
            }
        }

        private bool CanAnyWay(object p) => true;

        public ICommand SetDateDiapasone { get; }
        private void OnSetDateDiapasone(object p)
        {
            if (System.Convert.ToDouble(p) == -1)
            {
                FirstDate = new DateTime(2000, 1, 1);
                return;
            }
            LastDate = DateTime.Now;
            FirstDate = DateTime.Now.AddDays(-System.Convert.ToDouble(p));
        }


        private bool _isObjectSelected;
        public bool IsObjectSelected
        {
            get => _isObjectSelected;
            set => Set(ref _isObjectSelected, value);
        }


        private ObservableCollection<Appeal> _consultations;
        public ObservableCollection<Appeal> Consultations
        {
            get => _consultations;
            set => Set(ref _consultations, value);

        }

        private ObservableCollection<Appeal> _filteredConsultations;
        public ObservableCollection<Appeal> FilteredConsultations
        {
            get => _filteredConsultations;
            set
            {
                value = new(value.OrderByDescending(c => c.Created));
                Set(ref _filteredConsultations, value);

            }
        }
    }
}