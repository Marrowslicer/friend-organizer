using System;
using System.Threading.Tasks;

using FriendOrganizer.Model;
using FriendOrganizer.UI.Data.Repositories;
using FriendOrganizer.UI.View.Services;
using FriendOrganizer.UI.Wrapper;

using Prism.Commands;
using Prism.Events;

namespace FriendOrganizer.UI.ViewModel
{
    public class MeetingDetailViewModel : DetailViewModelBase, IMeetingDetailViewModel
    {
        private IMessageDialogService m_messageDialogService;
        private IMeetingRepository m_meetingRepository;
        private MeetingWrapper m_meeting;

        public MeetingDetailViewModel(
            IEventAggregator eventAggregator,
            IMessageDialogService messageDialogService,
            IMeetingRepository meetingRepository)
            : base(eventAggregator)
        {
            m_messageDialogService = messageDialogService;
            m_meetingRepository = meetingRepository;
        }

        public MeetingWrapper Meeting
        {
            get => m_meeting;
            private set
            {
                m_meeting = value;
                OnPropertyChanged();
            }
        }

        public override async Task LoadAsync(int? meetingId)
        {
            var meeting = meetingId.HasValue
                ? await m_meetingRepository.GetByIdAsync(meetingId.Value)
                : CreatNewMeeting();

            InitializeMeeting(meeting);
        }

        private Meeting CreatNewMeeting()
        {
            var meeting = new Meeting
            {
                DateFrom = DateTime.Now.Date,
                DateTo = DateTime.Now.Date
            };

            m_meetingRepository.Add(meeting);

            return meeting;
        }

        private void InitializeMeeting(Meeting meeting)
        {
            Meeting = new MeetingWrapper(meeting);
            Meeting.PropertyChanged += (s, e) =>
            {
                if (!HasChanges)
                {
                    HasChanges = m_meetingRepository.HasChanges();
                }

                if (e.PropertyName == nameof(Meeting.HasErrors))
                {
                    ((DelegateCommand)SaveCommand).RaiseCanExecuteChanged();
                }
            };

            ((DelegateCommand)SaveCommand).RaiseCanExecuteChanged();
        }

        protected override void OnDeleteExecute()
        {
            var result = m_messageDialogService.ShowOkCancelDialog(
                $"Do you really want to delete the meeting {Meeting.Title}?",
                "Question");

            if (result == MessageDialogResult.OK)
            {
                m_meetingRepository.Remove(Meeting.Model);
                m_meetingRepository.SaveAsync();
                RaiseDetailDeletedEvent(Meeting.Id);
            }
        }

        protected override bool OnSaveCanExecute()
        {
            return Meeting != null && !Meeting.HasErrors && HasChanges;
        }

        protected override async void OnSaveExecute()
        {
            await m_meetingRepository.SaveAsync();
            HasChanges = m_meetingRepository.HasChanges();
            RaiseDetailSavedEvent(Meeting.Id, Meeting.Title);
        }
    }
}
