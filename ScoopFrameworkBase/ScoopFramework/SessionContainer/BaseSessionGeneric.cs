using System;
using System.Collections.Generic;
using System.Linq;
using ScoopFramework.SessionContainer.Model;

namespace ScoopFramework.SessionContainer
{
    //List Session

    /// <summary>
    /// Amaç Başka formda kullanılan Listin içindeki dataya başka sayfada ulaşmak :)
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public static class BaseSessionGeneric<T>
    {
        private static readonly List<SessionModel<T>> _customList;

        public static bool CreateSessionList(Dictionary<string, IEnumerable<T>> dictionary)
        {
            var model = new SessionModel<T> { Key = dictionary.Keys.ToString() };

            model.Value.AddRange(dictionary.Values.Cast<T>());

            _customList.Add(model);

            return true;
        }

        public static bool RemoveSessionList(string sessionName)
        {
            try
            {
                for (var i = 0; i < _customList.Count; i++)
                {
                    if (_customList[i].Key.Equals(sessionName))
                    {
                        _customList.RemoveAt(i);
                    }
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
            return true;
        }

        public static SessionModel<T> GetSessionList(string sessionName)
        {
            var value = new SessionModel<T>();

            var getSessionList = _customList.FirstOrDefault(p => p.Key.Equals(sessionName));

            if (getSessionList == null) return value;

            value = getSessionList;

            return value;
        }

        //Burası Çok iyi Test Edilmeli
        public static List<SessionModel<T>> UpdateSession(string sessionName, List<T> updateDate)
        {
            try
            {
                foreach (SessionModel<T> t in _customList)
                {
                    if (t.Key.Equals(sessionName))
                    {
                        t.Value.AddRange(
                            new SessionModel<T> { Value = updateDate, Key = sessionName } as IEnumerable<T>);
                    }
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
            return _customList;
        }
    }
}
