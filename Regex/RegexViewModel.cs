using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace Regex
{
    public  class RegexViewModel : ViewModelBase
    {
        public RegexViewModel()
        {
            IsValidRegExp = true;
            IsValidMatchSource = true;
            GroupCollection = new ObservableCollection<RegexGroup>();
        }

        private System.Text.RegularExpressions.Match _match;

        private RequestCommand _verifyRegexCommand;
        public ICommand VerifyRegexCommand
        {
            get { return _verifyRegexCommand ?? new RequestCommand(param => this.VerifyRegex()); }
        }

        public string MatchSource
        {
            get;
            private set;
        }

        private bool _isValidMatchSource;
        [DefaultValue(true)]
        public bool IsValidMatchSource
        {
            get { return _isValidMatchSource; }
            set
            {
                _isValidMatchSource = value;
                OnPropertyChanged("IsValidMatchSource");
            }
        }

        public string RegExp
        {
            get;
            private set;
        }

        private bool _isValidRegExp;

        [DefaultValue(true)]
        public bool IsValidRegExp
        {
            get { return _isValidRegExp; }
            set
            {
                _isValidRegExp = value;
                OnPropertyChanged("IsValidRegExp");
            }
        }

        private string _matchResult;
        public string MatchResult
        {
            get { return _matchResult; }
            set
            {
                _matchResult = value;
                OnPropertyChanged("MatchResult");
            }
        }

        #region Groups
        private ObservableCollection<RegexGroup> _groupCollection;

        public ObservableCollection<RegexGroup> GroupCollection
        {
            get { return _groupCollection; }
            set
            {
                _groupCollection = value;
                OnPropertyChanged("GroupCollection");
            }
        }

        private RegexGroup _selectedGroup;
        public RegexGroup SelectedGroup
        {
            get { return _selectedGroup; }
            set
            {
                _selectedGroup = value;
                OnPropertyChanged("SelectedGroup");
            }
        }

        private RequestCommand _addGroupCommand;

        public RequestCommand AddGroupCommand
        {
            get { return _addGroupCommand?? new RequestCommand(para
                => this.AddGroup());
            }
        }

        private RequestCommand _removeGroupCommand;

        public RequestCommand RemoveGroupCommand
        {
            get { return _removeGroupCommand??new RequestCommand(param => this.RemoveGroup()); }
        }

        private void AddGroup()
        {
            RegexGroup group = new RegexGroup();
            GroupCollection.Add(group);
            SelectedGroup = group;
        }

        private void RemoveGroup()
        {
            if (SelectedGroup != null)
            {
                GroupCollection.Remove(SelectedGroup);
            }
        }
        #endregion




        public bool CanVerifyRegex()
        {

            if (string.IsNullOrWhiteSpace(RegExp))
            {
                IsValidRegExp = false;
            }

            if (string.IsNullOrWhiteSpace(MatchSource))
            {
                IsValidMatchSource = false;
            }

            if (IsValidRegExp & IsValidMatchSource == false)
            { return false; }

            _match = System.Text.RegularExpressions.Regex.Match(MatchSource, RegExp);
            return _match.Success;
        }

        public void VerifyRegex()
        {
            try
            {
                IsValidMatchSource = !string.IsNullOrWhiteSpace(MatchSource);
                IsValidRegExp = !string.IsNullOrWhiteSpace(RegExp);

                if ((IsValidRegExp & IsValidMatchSource) == false)
                { return; }

                // write the regular expression into local txt file
                try
                {
                    string localFile = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "regex.txt");
                    using (FileStream fs = new FileStream(localFile, FileMode.OpenOrCreate, FileAccess.ReadWrite))
                    {
                        using (StreamWriter sw = new StreamWriter(fs))
                        {
                            sw.WriteLine(RegExp);
                        }
                    }
                }
                catch (Exception)
                {
                }
                System.Text.RegularExpressions.Match match = System.Text.RegularExpressions.Regex.Match(MatchSource, RegExp);

                if (GroupCollection.Count == 0 || match.Groups.Count == 0)
                {
                    MatchResult = match.Value;
                }
                else
                {
                    StringBuilder groupBuilder = new StringBuilder();
                    if (GroupCollection.Count > 0)
                    {
                        foreach (var group in GroupCollection)
                        {
                            if(!string.IsNullOrWhiteSpace(group.GroupName))
                            {
                                groupBuilder.AppendFormat("{0} ", match.Groups[group.GroupName].Value );
                            }
                        }
                    }
                    MatchResult = groupBuilder.ToString();
                }
            }
            catch (Exception)
            {
            }
        }
    }

    public class RegexGroup
    {
        public string GroupName{get;set;}
    }
}
