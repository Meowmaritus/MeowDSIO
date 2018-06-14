//using System;
//using System.Collections;
//using System.Collections.Generic;
//using System.Collections.ObjectModel;
//using System.Linq;
//using System.Text;
//using System.Threading.Tasks;

//namespace MeowDSIO.DataTypes.MSB
//{
//    public class MsbDataTemplate : IList<MsbDataTemplateEntry>
//    {
//        private List<MsbDataTemplateEntry> entries;

//        private static readonly Dictionary<MsbParamSubType, MsbDataTemplate> DefaultTemplates
//            = new Dictionary<MsbParamSubType, MsbDataTemplate>();

//        private static MsbDataTemplate GenerateDefaultTemplate(MsbParamSubType type)
//        {
//            switch (type)
//            {
//                case MsbParamSubType.Model: return new MsbDataTemplate()
//                {
//                    new MsbDataTemplateEntry("Name Offset", MsbValueType.Int, "Name"),
//                    new MsbDataTemplateEntry("Type", MsbValueType.Int, "Name"),
//                };
//            }
//        }

//        public static MsbDataTemplate GetDefault(MsbParamSubType type)
//        {

//        }



//        #region IList
//        public MsbDataTemplateEntry this[int index] { get => ((IList<MsbDataTemplateEntry>)entries)[index]; set => ((IList<MsbDataTemplateEntry>)entries)[index] = value; }

//        public int Count => ((IList<MsbDataTemplateEntry>)entries).Count;

//        public bool IsReadOnly => ((IList<MsbDataTemplateEntry>)entries).IsReadOnly;

//        public void Add(MsbDataTemplateEntry item)
//        {
//            ((IList<MsbDataTemplateEntry>)entries).Add(item);
//        }

//        public void Clear()
//        {
//            ((IList<MsbDataTemplateEntry>)entries).Clear();
//        }

//        public bool Contains(MsbDataTemplateEntry item)
//        {
//            return ((IList<MsbDataTemplateEntry>)entries).Contains(item);
//        }

//        public void CopyTo(MsbDataTemplateEntry[] array, int arrayIndex)
//        {
//            ((IList<MsbDataTemplateEntry>)entries).CopyTo(array, arrayIndex);
//        }

//        public IEnumerator<MsbDataTemplateEntry> GetEnumerator()
//        {
//            return ((IList<MsbDataTemplateEntry>)entries).GetEnumerator();
//        }

//        public int IndexOf(MsbDataTemplateEntry item)
//        {
//            return ((IList<MsbDataTemplateEntry>)entries).IndexOf(item);
//        }

//        public void Insert(int index, MsbDataTemplateEntry item)
//        {
//            ((IList<MsbDataTemplateEntry>)entries).Insert(index, item);
//        }

//        public bool Remove(MsbDataTemplateEntry item)
//        {
//            return ((IList<MsbDataTemplateEntry>)entries).Remove(item);
//        }

//        public void RemoveAt(int index)
//        {
//            ((IList<MsbDataTemplateEntry>)entries).RemoveAt(index);
//        }

//        IEnumerator IEnumerable.GetEnumerator()
//        {
//            return ((IList<MsbDataTemplateEntry>)entries).GetEnumerator();
//        }
//        #endregion
//    }
//}
