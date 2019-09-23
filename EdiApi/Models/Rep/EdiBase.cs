using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel;
using System.Security.Cryptography;
using EdiApi.Models;
using System.Reflection;
using ComModels.Models.EdiDB;

namespace EdiApi
{
    [Browsable(true)]
    public partial class EdiBase : EdiSec
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
        public EdiBase(string _SegmentTerminator) { SegmentTerminator = _SegmentTerminator; HashId = GetHashId(); }
        public string Ts()
        {
            string Ret = string.Empty;
            foreach (string OrdenO in Orden)
            {
                if (OrdenO == "Init")
                    Ret += $"{this.GetType().GetField("Init").GetRawConstantValue()}{ElementTerminator}";
                else
                    Ret += $"{this.GetType().GetProperty(OrdenO).GetValue(this, null)}{ElementTerminator}";
            }
            Ret = Ret.TrimEnd(ElementTerminator[0]) + SegmentTerminator + Environment.NewLine;
            foreach (EdiBase ChildO in this.Childs)
                Ret += ChildO.Ts();
            return Ret;
        }
        public static string GetHashId()
        {
            return $"H{Guid.NewGuid().ToString().Replace("-", "").Substring(0, 24)}{DateTime.Now.ToString("ddMMyyyyHHmm")}";
        }
        public bool Parse(string _EdiStr)
        {
            try
            {
                EdiSec.CheckSeg++;
                Coli = 0;
                //HashId = $"H{GetHashId()}";
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
        public void SaveAll830(ref EdiDBContext _DbO) {
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
                    case N1856.Init:
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
                        _DbO.LearShp830.Add(Reflect(new LearShp830()));
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
                throw new Exception($"Error al guardar {GetType().GetField("Init").GetRawConstantValue() } {eFb1.ToString()}. Error en linea {EdiStr}");
            }            
            foreach (EdiBase ChildO in this.Childs)
                ChildO.SaveAll830(ref _DbO);
        }
        public void SaveAll856(ref EdiDBContext _DbO)
        {
            try
            {
                ApplicationSettings.SavedSegments++;
                Validate();
                switch (GetType().GetField("Init").GetRawConstantValue())
                {   
                    case ISA856.Init:
                        LearRep856.LearIsa856root = Reflect(new LearIsa856());
                        _DbO.LearIsa856.Add(LearRep856.LearIsa856root);
                        break;
                    case BSN856.Init:
                        _DbO.LearBsn856.Add(Reflect(new LearBsn856()));
                        break;
                    case CLD856.Init:
                        _DbO.LearCld856.Add(Reflect(new LearCld856()));
                        break;
                    case CTT856.Init:
                        _DbO.LearCtt856.Add(Reflect(new LearCtt856()));
                        break;
                    case DTM856.Init:
                        _DbO.LearDtm856.Add(Reflect(new LearDtm856()));
                        break;
                    case ETD856.Init:
                        _DbO.LearEtd856.Add(Reflect(new LearEtd856()));
                        break;
                    //case GE856.Init:
                    //    break;
                    case GS856.Init:
                        _DbO.LearGs856.Add(Reflect(new LearGs856()));
                        break;
                    case HLOL856.Init:
                        if (this.GetType().Name == "HLOL856")
                            _DbO.LearHlol856.Add(Reflect(new LearHlol856()));
                        else
                            _DbO.LearHlsl856.Add(Reflect(new LearHlsl856()));
                        break;
                    //case IEA856.Init:
                    //    _DbO.LearI
                    //    break;
                    case LIN856.Init:
                        _DbO.LearLin856.Add(Reflect(new LearLin856()));
                        break;
                    case MEA856.Init:
                        _DbO.LearMea856.Add(Reflect(new LearMea856()));
                        break;
                    case N1856.Init:
                        _DbO.LearN1856.Add(Reflect(new LearN1856()));
                        break;
                    case PRF856.Init:
                        _DbO.LearPrf856.Add(Reflect(new LearPrf856()));
                        break;
                    case REF856.Init:
                        _DbO.LearRef856.Add(Reflect(new LearRef856()));
                        break;
                    //case SE856.Init:
                    //    _DbO.LearSE
                    //    break;
                    case SN1856.Init:
                        _DbO.LearSn1856.Add(Reflect(new LearSn1856()));
                        break;
                    case ST856.Init:
                        _DbO.LearSt856.Add(Reflect(new LearSt856()));
                        break;
                    case TD1856.Init:
                        _DbO.LearTd1856.Add(Reflect(new LearTd1856()));
                        break;
                    case TD3856.Init:
                        _DbO.LearTd3856.Add(Reflect(new LearTd3856()));
                        break;
                    case TD5856.Init:
                        _DbO.LearTd5856.Add(Reflect(new LearTd5856()));
                        break;
                    default:
                        break;
                }
                _DbO.SaveChanges();
            }
            catch (Exception eFb1)
            {
                throw new Exception($"Error al guardar {GetType().GetField("Init").GetRawConstantValue() } {eFb1.ToString()}. Error en linea {EdiStr}");
            }
            foreach (EdiBase ChildO in this.Childs)
                ChildO.SaveAll856(ref _DbO);
        }
    }
}
