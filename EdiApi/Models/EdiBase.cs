using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel;
using System.Security.Cryptography;
using EdiApi.Models;
using System.Reflection;

namespace EdiApi
{
    [Browsable(true)]
    public class EdiBase : EdiSec
    {
        public int Coli { set; get; }
        public string NotUsed { set; get; } = "";        
        public string[] EdiArray { set; get; }
        public static string SegmentTerminator { get; set; } = "~";
        public static string ElementTerminator { get; set; } = "*";
        public string CompositeTerminator { get; set; } = ">";
        public IEnumerable<string> Orden { set; get; }
        public EdiBase Parent { get; set; }
        public string ParentHashId => Parent == null? string.Empty: Parent.HashId;
        public List<EdiBase> Childs { get; set; } = new List<EdiBase>();
        public EdiBase(string _SegmentTerminator) { SegmentTerminator = _SegmentTerminator; }
        public string Ts()
        {
            string Ret = string.Empty;
            foreach (string OrdenO in Orden)
                Ret += $"{this.GetType().GetProperty(OrdenO).GetValue(this, null)}{ElementTerminator}";
            Ret = Ret.TrimEnd(ElementTerminator[0]) + SegmentTerminator + Environment.NewLine;
            return Ret;
        }
        public static string GetHashId()
        {
            return Guid.NewGuid().ToString().Replace("-", "").Substring(0, 24) + DateTime.Now.Year.ToString();
        }
        public bool Parse(string _EdiStr)
        {
            try
            {
                EdiSec.CheckSeg++;
                Coli = 0;
                HashId = $"H{GetHashId()}";
                EdiStr = _EdiStr;
                EdiArray = EdiStr.Replace(SegmentTerminator, "").Split(ElementTerminator);
                //if (Orden.Count() != EdiArray.Length)
                //    return false;
                foreach (string OrdenO in Orden)
                {
                    if (OrdenO != "Init")
                    {
                        Coli++;
                        if (Coli == EdiArray.Length)
                            break;
                        this.GetType().GetProperty(OrdenO).SetValue(this, EdiArray[Coli]);
                    }
                    //Ret += $"{this.GetType().GetProperty(OrdenO).GetValue(this, null)}{ElementTerminator}";
                }
                return true;
            }
            catch
            {
                return false;
            }
        }        
        public string Validate()
        {
            string Ret = string.Empty;
            List<ValidationResult> LVal = new List<ValidationResult>();
            ValidationContext Context = new ValidationContext(this, null, null);
            Validator.TryValidateObject(this, Context, LVal, true);
            if (LVal.Count > 0)
                Ret = string.Join(',', LVal);
            if (!string.IsNullOrEmpty(Ret))
                throw new Exception(Ret);
            return Ret;
        }
        public T Reflect<T>(T _Dest)
        {
            foreach (PropertyInfo PropertyInfoO in this.GetType().GetProperties())
            {
                try
                {
                    if (_Dest.GetType().GetProperty(PropertyInfoO.Name) != null)
                        _Dest.GetType().GetProperty(PropertyInfoO.Name).SetValue(_Dest, this.GetType().GetProperty(PropertyInfoO.Name).GetValue(this));
                }
                catch (Exception er1)
                {
                    throw er1;
                }                
            }
            return _Dest;
        }
        public void SaveAll(ref EdiDBContext _DbO) {
            try
            {
                ApplicationSettings.SavedSegments++;
                Validate();
                switch (GetType().GetField("Init").GetRawConstantValue())
                {
                    case ISA830.Init:
                        LearRep830.LearIsa830root = Reflect(new LearIsa830());
                        _DbO.LearIsa830.Add(LearRep830.LearIsa830root);
                        break;
                    case GS830.Init:
                        _DbO.LearGs830.Add(Reflect(new LearGs830()));
                        break;
                    case ST830.Init:
                        _DbO.LearSt830.Add(Reflect(new LearSt830()));
                        break;
                    case BFR830.Init:
                        _DbO.LearBfr830.Add(Reflect(new LearBfr830()));
                        break;
                    case NTE830.Init:
                        _DbO.LearNte830.Add(Reflect(new LearNte830()));
                        break;
                    case N1830.Init:
                        _DbO.LearN1830.Add(Reflect(new LearN1830()));
                        break;
                    case N4830.Init:
                        _DbO.LearN4830.Add(Reflect(new LearN4830()));
                        break;
                    case LIN830.Init:
                        _DbO.LearLin830.Add(Reflect(new LearLin830()));
                        break;
                    case UIT830.Init:
                        _DbO.LearUit830.Add(Reflect(new LearUit830()));
                        break;
                    case PRS830.Init:
                        _DbO.LearPrs830.Add(Reflect(new LearPrs830()));
                        break;
                    case SDP830.Init:
                        _DbO.LearSdp830.Add(Reflect(new LearSdp830()));
                        break;
                    case FST830.Init:
                        _DbO.LearFst830.Add(Reflect(new LearFst830()));
                        break;
                    case ATH830.Init:
                        _DbO.LearAth830.Add(Reflect(new LearAth830()));
                        break;
                    case SHP830.Init:
                        _DbO.LearShp830.Add(Reflect(new LearShp830())); _DbO.SaveChanges();
                        break;
                    case REF830.Init:
                        _DbO.LearRef830.Add(Reflect(new LearRef830()));
                        break;
                    //case CTT830.Init:
                    //    break;
                    //case SE830.Init:
                    //    break;
                    //case GE830.Init:
                    //    break;
                    //case IEA830.Init:
                    //    break;
                    default:
                        break;
                }
            }
            catch (Exception eFb1) 
            {
                throw new Exception($"Error al guardar {GetType().GetField("Init").GetRawConstantValue() } {eFb1.ToString()}");
            }            
            foreach (EdiBase ChildO in this.Childs)
                ChildO.SaveAll(ref _DbO);
        }
    }
}
