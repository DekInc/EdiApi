using ComModels.Models.EdiDB;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Threading.Tasks;

namespace EdiApi.Models
{
    /// <summary>
    /// https://www.codeproject.com/Tips/443588/Simple-Csharp-FTP-Class
    /// </summary>
    public class ComRepoFtp : ComS
    {
        private const string Id = "FTP";
        private readonly string host = null;
        private readonly string host2 = null;
        private readonly string user = null;
        private readonly string pass = null;
        private readonly string DirIn = null;
        private readonly string DirOut = null;
        private readonly string DirChecked = null;
        public bool UseHost2 = false;
        object MaxEdiComs { set; get; }
        public string[] Files { set; get; }
        private FtpWebRequest ftpRequest = null;
        private FtpWebResponse ftpResponse = null;
        private Stream ftpStream = null;
        private const int bufferSize = 2048;
        /* Construct Object */
        public ComRepoFtp(string hostIP, string hostIP2, string userName, string password, string _DirIn, string _DireOut, string _DirChecked, object _MaxEdiComs) {
            host = hostIP;
            host2 = hostIP2;
            user = userName;
            pass = password;
            DirIn = _DirIn;
            DirOut = _DireOut;
            MaxEdiComs = _MaxEdiComs;
            DirChecked = _DirChecked;
        }

        public bool Ping(ref EdiDBContext _DbO)
        {            
            try
            {
                CheckMaxEdiComs(ref _DbO, MaxEdiComs);
                ftpRequest = UseHost2? (FtpWebRequest)WebRequest.Create(host2 + "/" + DirIn):(FtpWebRequest)WebRequest.Create(host + "/" + DirIn);
                ftpRequest.Credentials = new NetworkCredential(user, pass);
                ftpRequest.UseBinary = true;
                ftpRequest.UsePassive = true;
                ftpRequest.KeepAlive = true;
                ftpRequest.Method = WebRequestMethods.Ftp.ListDirectory;
                ftpResponse = (FtpWebResponse)ftpRequest.GetResponse();
                ftpResponse.Close();
                ftpRequest = null;
                AddComLog(ref _DbO, Id, $"Ping hacía ftp {(UseHost2 ? host2 : host)} correcto");
                return true;
            }
            catch (WebException we1)
            {
                if (we1.Message.Contains("File unavailable"))
                {
                    AddComLog(ref _DbO, Id, $"Ping hacía ftp {(UseHost2 ? host2 : host)} correcto");
                    return true;
                }
                else {
                    AddComLog(ref _DbO, Id, $"No se puede comunicar con el ftp {(UseHost2 ? host2 : host)}. Info: {we1.ToString()}");
                    return false;
                }
            }
            catch (Exception ex) {
                AddComLog(ref _DbO, Id, $"No se puede comunicar con el ftp {(UseHost2 ? host2 : host)}. Info: {ex.ToString()}");
                return false;
            }
        }

        public void GetInStack(ref EdiDBContext _DbO, ref int _CodError, ref string _MessageSubject)
        {
            try
            {
                CheckMaxEdiComs(ref _DbO, MaxEdiComs);
                ftpRequest = UseHost2 ? (FtpWebRequest)WebRequest.Create(host2 + "/" + DirIn) : (FtpWebRequest)WebRequest.Create(host + "/" + DirIn);
                ftpRequest.Credentials = new NetworkCredential(user, pass);
                ftpRequest.UseBinary = true;
                ftpRequest.UsePassive = true;
                ftpRequest.KeepAlive = true;
                ftpRequest.Method = WebRequestMethods.Ftp.ListDirectory;
                ftpResponse = (FtpWebResponse)ftpRequest.GetResponse();
                ftpStream = ftpResponse.GetResponseStream();
                StreamReader ftpReader = new StreamReader(ftpStream);
                string directoryRaw = null;
                try
                {
                    while (ftpReader.Peek() != -1)
                    {
                        directoryRaw += ftpReader.ReadLine() + "|";
                    }
                }
                catch (Exception ex)
                {
                    _CodError = -4;
                    _MessageSubject = $"Error al listar archivos de ftp {(UseHost2 ? host2 : host)}. Info: {ex.ToString()}";
                    AddComLog(ref _DbO, Id, _MessageSubject);
                    Files = new string[] { "" };
                }
                ftpReader.Close();
                ftpStream.Close();
                ftpResponse.Close();
                ftpRequest = null;
                try
                {
                    if (directoryRaw == null)
                    {
                        _CodError = -5;
                        _MessageSubject = $"No hay archivos a procesar en {(UseHost2 ? host2 : host)}";
                        AddComLog(ref _DbO, Id, _MessageSubject);
                        Files = new string[] { "" };
                        return;
                    }
                    string[] directoryList = directoryRaw.Split("|".ToCharArray());
                    Files = directoryList;
                    if (Files.LastOrDefault() == "")
                        Files = Files.Where((F, Fi) => Fi != (Files.Length - 1)).ToArray();
                }
                catch (Exception ex)
                {
                    _CodError = -6;
                    _MessageSubject = $"Error al listar archivos del directorio de ftp {(UseHost2 ? host2 : host)}. Info: {ex.ToString()}";
                    AddComLog(ref _DbO, Id, _MessageSubject);
                    Files = new string[] { "" };
                }
            }
            catch (Exception ex)
            {
                _CodError = -7;
                _MessageSubject = $"Error en metodo listar archivos a nivel de conexión del directorio de ftp {(UseHost2 ? host2 : host)}. Info: {ex.ToString()}";
                AddComLog(ref _DbO, Id, _MessageSubject);
                Files = new string[] { "" };
            }
        }

        public void Put(string _FileName, string localFile, ref EdiDBContext _DbO)
        {
            try
            {
                CheckMaxEdiComs(ref _DbO, MaxEdiComs);
                ftpRequest = UseHost2 ? (FtpWebRequest)WebRequest.Create($"{host2}/{DirOut}/{_FileName}") : (FtpWebRequest)WebRequest.Create($"{host}/{DirOut}/{_FileName}");
                ftpRequest.Credentials = new NetworkCredential(user, pass);
                ftpRequest.UseBinary = true;
                ftpRequest.UsePassive = true;
                ftpRequest.KeepAlive = true;
                ftpRequest.Method = WebRequestMethods.Ftp.UploadFile;
                ftpStream = ftpRequest.GetRequestStream();
                FileStream localFileStream = new FileStream(localFile, FileMode.Open);
                byte[] byteBuffer = new byte[bufferSize];
                int bytesSent = localFileStream.Read(byteBuffer, 0, bufferSize);
                try
                {
                    while (bytesSent != 0)
                    {
                        ftpStream.Write(byteBuffer, 0, bytesSent);
                        bytesSent = localFileStream.Read(byteBuffer, 0, bufferSize);
                    }
                    AddComLog(ref _DbO, Id, $"Archivo enviado {_FileName} en {(UseHost2 ? host2 : host)}");
                }
                catch (Exception ex) {
                    AddComLog(ref _DbO, Id, $"Error al procesar el archivo en {(UseHost2 ? host2 : host)}. Info: {ex.ToString()}");
                }
                localFileStream.Close();
                ftpStream.Close();
                ftpRequest = null;
            }
            catch (Exception ex) {
                AddComLog(ref _DbO, Id, $"Error relacionado con la conexión de {(UseHost2 ? host2 : host)}. Info: {ex.ToString()}");
            }
            return;
        }
        public void Put(string _EdiStr, ref EdiDBContext _DbO, string Tipo)
        {
            try
            {
                string FtpFile = EdiBase.GetHashId();
                StreamWriter Sw = new StreamWriter(FtpFile, false);
                Sw.Write(_EdiStr);
                Sw.Close();
                CheckMaxEdiComs(ref _DbO, MaxEdiComs);
                ftpRequest = UseHost2 ? (FtpWebRequest)WebRequest.Create($"{host2}/{DirOut}/{FtpFile}") : (FtpWebRequest)WebRequest.Create($"{host}/{DirOut}/{FtpFile}");
                ftpRequest.Credentials = new NetworkCredential(user, pass);
                ftpRequest.UseBinary = true;
                ftpRequest.UsePassive = true;
                ftpRequest.KeepAlive = true;
                ftpRequest.Method = WebRequestMethods.Ftp.UploadFile;
                ftpStream = ftpRequest.GetRequestStream();
                FileStream localFileStream = new FileStream(FtpFile, FileMode.Open);
                byte[] byteBuffer = new byte[bufferSize];
                int bytesSent = localFileStream.Read(byteBuffer, 0, bufferSize);
                try
                {
                    while (bytesSent != 0)
                    {
                        ftpStream.Write(byteBuffer, 0, bytesSent);
                        bytesSent = localFileStream.Read(byteBuffer, 0, bufferSize);
                    }
                    AddComLog(ref _DbO, Id, $"Archivo enviado en {(UseHost2 ? host2 : host)}. Tipo: {Tipo}");
                }
                catch (Exception ex)
                {
                    AddComLog(ref _DbO, Id, $"Error al procesar el archivo en {(UseHost2 ? host2 : host)}. Info: {ex.ToString()}. Tipo: {Tipo}");
                }
                localFileStream.Close();
                ftpStream.Close();
                ftpRequest = null;
                File.Delete(FtpFile);
            }
            catch (Exception ex)
            {
                AddComLog(ref _DbO, Id, $"Error relacionado con la conexión de {(UseHost2 ? host2 : host)}. Info: {ex.ToString()}. Tipo: {Tipo}");
            }
            return;
        }

        public void Get(ref EdiDBContext _DbO, ref int _CodError, ref string _MessageSubject, ref string _FileName, ref List<string> _EdiPure)
        {
            try
            {
                GetInStack(ref _DbO, ref _CodError, ref _MessageSubject);
                if (Files.Length == 0)
                    return;
                if (string.IsNullOrEmpty(Files.LastOrDefault()))
                    return;
                List<string> ListFiles = Files.ToList();
                List<LearPureEdi> ListLearPureEdiO = (
                    from Pe in _DbO.LearPureEdi
                    from Lf in ListFiles
                    where Pe.NombreArchivo == Lf
                    select Pe).ToList();
                for(int Ci = 0; Ci < ListLearPureEdiO.Count; Ci++)
                {
                    if (ListLearPureEdiO[Ci].Reprocesar)
                    {
                        ListLearPureEdiO[Ci].Reprocesar = false;
                        _DbO.LearPureEdi.Update(ListLearPureEdiO[Ci]);
                        _DbO.SaveChanges();
                        _FileName = ListLearPureEdiO[Ci].NombreArchivo;                        
                        break;
                    }
                    ListFiles.Remove(ListLearPureEdiO[Ci].NombreArchivo);
                }
                if (string.IsNullOrEmpty(_FileName))
                {
                    if (ListFiles.Count == 0)
                    {
                        _CodError = -2;
                        return;
                    }
                    else {
                        _FileName = ListFiles.Fod();
                    }
                }
                ftpRequest = UseHost2 ? (FtpWebRequest)WebRequest.Create($"{host2}/{DirIn}/{_FileName}") : (FtpWebRequest)WebRequest.Create($"{host}/{DirIn}/{_FileName}");
                ftpRequest.Credentials = new NetworkCredential(user, pass);
                ftpRequest.UseBinary = true;
                ftpRequest.UsePassive = true;
                ftpRequest.KeepAlive = true;
                ftpRequest.Method = WebRequestMethods.Ftp.DownloadFile;
                ftpResponse = (FtpWebResponse)ftpRequest.GetResponse();
                ftpStream = ftpResponse.GetResponseStream();
                MemoryStream localFileStream = new MemoryStream();
                byte[] byteBuffer = new byte[bufferSize];
                int bytesRead = ftpStream.Read(byteBuffer, 0, bufferSize);
                try
                {
                    while (bytesRead > 0)
                    {
                        localFileStream.Write(byteBuffer, 0, bytesRead);
                        bytesRead = ftpStream.Read(byteBuffer, 0, bufferSize);                        
                    }
                }
                catch (Exception ex)
                {
                    _CodError = -9;
                    _MessageSubject = $"Error, al escribir el archivo en stream de memoria {_FileName} en ftp {(UseHost2 ? host2 : host)}. Info: {ex.ToString()}";
                    AddComLog(ref _DbO, Id, _MessageSubject);
                    return;
                }
                localFileStream.Close();
                ftpStream.Close();
                ftpResponse.Close();
                ftpRequest = null;
                string EdiPure2 = System.Text.Encoding.UTF8.GetString(localFileStream.ToArray());
                _EdiPure = EdiPure2.Split(EdiBase.SegmentTerminator).ToList();
                if (_EdiPure.Count == 1)
                    _EdiPure = EdiPure2.Split('\n').ToList();
                AddComLog(ref _DbO, Id, $"Se obtuvo el archivo {_FileName} de ftp {(UseHost2 ? host2 : host)}");
                return;
            }
            catch (Exception ex)
            {
                _CodError = -10;
                _MessageSubject = $"Error en método GET en ftp {(UseHost2 ? host2 : host)}. Info: {ex.ToString()}";
                AddComLog(ref _DbO, Id, _MessageSubject);
                return;
            }
        }

        /* Download File */
        public void Download(string remoteFile, string localFile)
        {
            try
            {
                /* Create an FTP Request */
                ftpRequest = (FtpWebRequest)FtpWebRequest.Create(host + "/" + remoteFile);
                /* Log in to the FTP Server with the User Name and Password Provided */
                ftpRequest.Credentials = new NetworkCredential(user, pass);
                /* When in doubt, use these options */
                ftpRequest.UseBinary = true;
                ftpRequest.UsePassive = true;
                ftpRequest.KeepAlive = true;
                /* Specify the Type of FTP Request */
                ftpRequest.Method = WebRequestMethods.Ftp.DownloadFile;
                /* Establish Return Communication with the FTP Server */
                ftpResponse = (FtpWebResponse)ftpRequest.GetResponse();
                /* Get the FTP Server's Response Stream */
                ftpStream = ftpResponse.GetResponseStream();
                /* Open a File Stream to Write the Downloaded File */
                FileStream localFileStream = new FileStream(localFile, FileMode.Create);
                /* Buffer for the Downloaded Data */
                byte[] byteBuffer = new byte[bufferSize];
                int bytesRead = ftpStream.Read(byteBuffer, 0, bufferSize);
                /* Download the File by Writing the Buffered Data Until the Transfer is Complete */
                try
                {
                    while (bytesRead > 0)
                    {
                        localFileStream.Write(byteBuffer, 0, bytesRead);
                        bytesRead = ftpStream.Read(byteBuffer, 0, bufferSize);
                    }
                }
                catch (Exception ex) { Console.WriteLine(ex.ToString()); }
                /* Resource Cleanup */
                localFileStream.Close();
                ftpStream.Close();
                ftpResponse.Close();
                ftpRequest = null;
            }
            catch (Exception ex) { Console.WriteLine(ex.ToString()); }
            return;
        }

        /* Upload File */
        public void Upload(string remoteFile, string localFile)
        {
            try
            {
                /* Create an FTP Request */
                ftpRequest = (FtpWebRequest)FtpWebRequest.Create(host + "/" + remoteFile);
                /* Log in to the FTP Server with the User Name and Password Provided */
                ftpRequest.Credentials = new NetworkCredential(user, pass);
                /* When in doubt, use these options */
                ftpRequest.UseBinary = true;
                ftpRequest.UsePassive = true;
                ftpRequest.KeepAlive = true;
                /* Specify the Type of FTP Request */
                ftpRequest.Method = WebRequestMethods.Ftp.UploadFile;
                /* Establish Return Communication with the FTP Server */
                ftpStream = ftpRequest.GetRequestStream();
                /* Open a File Stream to Read the File for Upload */
                FileStream localFileStream = new FileStream(localFile, FileMode.Create);
                /* Buffer for the Downloaded Data */
                byte[] byteBuffer = new byte[bufferSize];
                int bytesSent = localFileStream.Read(byteBuffer, 0, bufferSize);
                /* Upload the File by Sending the Buffered Data Until the Transfer is Complete */
                try
                {
                    while (bytesSent != 0)
                    {
                        ftpStream.Write(byteBuffer, 0, bytesSent);
                        bytesSent = localFileStream.Read(byteBuffer, 0, bufferSize);
                    }
                }
                catch (Exception ex) { Console.WriteLine(ex.ToString()); }
                /* Resource Cleanup */
                localFileStream.Close();
                ftpStream.Close();
                ftpRequest = null;
            }
            catch (Exception ex) { Console.WriteLine(ex.ToString()); }
            return;
        }

        /* Delete File */
        public void Delete(string deleteFile)
        {
            try
            {
                /* Create an FTP Request */
                ftpRequest = (FtpWebRequest)WebRequest.Create(host + "/" + deleteFile);
                /* Log in to the FTP Server with the User Name and Password Provided */
                ftpRequest.Credentials = new NetworkCredential(user, pass);
                /* When in doubt, use these options */
                ftpRequest.UseBinary = true;
                ftpRequest.UsePassive = true;
                ftpRequest.KeepAlive = true;
                /* Specify the Type of FTP Request */
                ftpRequest.Method = WebRequestMethods.Ftp.DeleteFile;
                /* Establish Return Communication with the FTP Server */
                ftpResponse = (FtpWebResponse)ftpRequest.GetResponse();
                /* Resource Cleanup */
                ftpResponse.Close();
                ftpRequest = null;
            }
            catch (Exception ex) { Console.WriteLine(ex.ToString()); }
            return;
        }

        /* Rename File */
        public void Rename(string currentFileNameAndPath, string newFileName)
        {
            try
            {
                /* Create an FTP Request */
                ftpRequest = (FtpWebRequest)WebRequest.Create(host + "/" + currentFileNameAndPath);
                /* Log in to the FTP Server with the User Name and Password Provided */
                ftpRequest.Credentials = new NetworkCredential(user, pass);
                /* When in doubt, use these options */
                ftpRequest.UseBinary = true;
                ftpRequest.UsePassive = true;
                ftpRequest.KeepAlive = true;
                /* Specify the Type of FTP Request */
                ftpRequest.Method = WebRequestMethods.Ftp.Rename;
                /* Rename the File */
                ftpRequest.RenameTo = newFileName;
                /* Establish Return Communication with the FTP Server */
                ftpResponse = (FtpWebResponse)ftpRequest.GetResponse();
                /* Resource Cleanup */
                ftpResponse.Close();
                ftpRequest = null;
            }
            catch (Exception ex) { Console.WriteLine(ex.ToString()); }
            return;
        }

        /* Create a New Directory on the FTP Server */
        public void CreateDirectory(string newDirectory)
        {
            try
            {
                /* Create an FTP Request */
                ftpRequest = (FtpWebRequest)WebRequest.Create(host + "/" + newDirectory);
                /* Log in to the FTP Server with the User Name and Password Provided */
                ftpRequest.Credentials = new NetworkCredential(user, pass);
                /* When in doubt, use these options */
                ftpRequest.UseBinary = true;
                ftpRequest.UsePassive = true;
                ftpRequest.KeepAlive = true;
                /* Specify the Type of FTP Request */
                ftpRequest.Method = WebRequestMethods.Ftp.MakeDirectory;
                /* Establish Return Communication with the FTP Server */
                ftpResponse = (FtpWebResponse)ftpRequest.GetResponse();
                /* Resource Cleanup */
                ftpResponse.Close();
                ftpRequest = null;
            }
            catch (Exception ex) { Console.WriteLine(ex.ToString()); }
            return;
        }

        /* Get the Date/Time a File was Created */
        public string GetFileCreatedDateTime(string fileName)
        {
            try
            {
                /* Create an FTP Request */
                ftpRequest = (FtpWebRequest)FtpWebRequest.Create(host + "/" + fileName);
                /* Log in to the FTP Server with the User Name and Password Provided */
                ftpRequest.Credentials = new NetworkCredential(user, pass);
                /* When in doubt, use these options */
                ftpRequest.UseBinary = true;
                ftpRequest.UsePassive = true;
                ftpRequest.KeepAlive = true;
                /* Specify the Type of FTP Request */
                ftpRequest.Method = WebRequestMethods.Ftp.GetDateTimestamp;
                /* Establish Return Communication with the FTP Server */
                ftpResponse = (FtpWebResponse)ftpRequest.GetResponse();
                /* Establish Return Communication with the FTP Server */
                ftpStream = ftpResponse.GetResponseStream();
                /* Get the FTP Server's Response Stream */
                StreamReader ftpReader = new StreamReader(ftpStream);
                /* Store the Raw Response */
                string fileInfo = null;
                /* Read the Full Response Stream */
                try { fileInfo = ftpReader.ReadToEnd(); }
                catch (Exception ex) { Console.WriteLine(ex.ToString()); }
                /* Resource Cleanup */
                ftpReader.Close();
                ftpStream.Close();
                ftpResponse.Close();
                ftpRequest = null;
                /* Return File Created Date Time */
                return fileInfo;
            }
            catch (Exception ex) { Console.WriteLine(ex.ToString()); }
            /* Return an Empty string Array if an Exception Occurs */
            return "";
        }

        /* Get the Size of a File */
        public string GetFileSize(string fileName)
        {
            try
            {
                /* Create an FTP Request */
                ftpRequest = (FtpWebRequest)FtpWebRequest.Create(host + "/" + fileName);
                /* Log in to the FTP Server with the User Name and Password Provided */
                ftpRequest.Credentials = new NetworkCredential(user, pass);
                /* When in doubt, use these options */
                ftpRequest.UseBinary = true;
                ftpRequest.UsePassive = true;
                ftpRequest.KeepAlive = true;
                /* Specify the Type of FTP Request */
                ftpRequest.Method = WebRequestMethods.Ftp.GetFileSize;
                /* Establish Return Communication with the FTP Server */
                ftpResponse = (FtpWebResponse)ftpRequest.GetResponse();
                /* Establish Return Communication with the FTP Server */
                ftpStream = ftpResponse.GetResponseStream();
                /* Get the FTP Server's Response Stream */
                StreamReader ftpReader = new StreamReader(ftpStream);
                /* Store the Raw Response */
                string fileInfo = null;
                /* Read the Full Response Stream */
                try { while (ftpReader.Peek() != -1) { fileInfo = ftpReader.ReadToEnd(); } }
                catch (Exception ex) { Console.WriteLine(ex.ToString()); }
                /* Resource Cleanup */
                ftpReader.Close();
                ftpStream.Close();
                ftpResponse.Close();
                ftpRequest = null;
                /* Return File Size */
                return fileInfo;
            }
            catch (Exception ex) { Console.WriteLine(ex.ToString()); }
            /* Return an Empty string Array if an Exception Occurs */
            return "";
        }

        /* List Directory Contents File/Folder Name Only */
        public string[] DirectoryListSimple(string directory)
        {
            try
            {
                /* Create an FTP Request */
                ftpRequest = (FtpWebRequest)FtpWebRequest.Create(host + "/" + directory);
                /* Log in to the FTP Server with the User Name and Password Provided */
                ftpRequest.Credentials = new NetworkCredential(user, pass);
                /* When in doubt, use these options */
                ftpRequest.UseBinary = true;
                ftpRequest.UsePassive = true;
                ftpRequest.KeepAlive = true;
                /* Specify the Type of FTP Request */
                ftpRequest.Method = WebRequestMethods.Ftp.ListDirectory;
                /* Establish Return Communication with the FTP Server */
                ftpResponse = (FtpWebResponse)ftpRequest.GetResponse();
                /* Establish Return Communication with the FTP Server */
                ftpStream = ftpResponse.GetResponseStream();
                /* Get the FTP Server's Response Stream */
                StreamReader ftpReader = new StreamReader(ftpStream);
                /* Store the Raw Response */
                string directoryRaw = null;
                /* Read Each Line of the Response and Append a Pipe to Each Line for Easy Parsing */
                try { while (ftpReader.Peek() != -1) { directoryRaw += ftpReader.ReadLine() + "|"; } }
                catch (Exception ex) { Console.WriteLine(ex.ToString()); }
                /* Resource Cleanup */
                ftpReader.Close();
                ftpStream.Close();
                ftpResponse.Close();
                ftpRequest = null;
                /* Return the Directory Listing as a string Array by Parsing 'directoryRaw' with the Delimiter you Append (I use | in This Example) */
                try { string[] directoryList = directoryRaw.Split("|".ToCharArray()); return directoryList; }
                catch (Exception ex) { Console.WriteLine(ex.ToString()); }
            }
            catch (Exception ex) { Console.WriteLine(ex.ToString()); }
            /* Return an Empty string Array if an Exception Occurs */
            return new string[] { "" };
        }

        /* List Directory Contents in Detail (Name, Size, Created, etc.) */
        public string[] DirectoryListDetailed(string directory)
        {
            try
            {
                /* Create an FTP Request */
                ftpRequest = (FtpWebRequest)FtpWebRequest.Create(host + "/" + directory);
                /* Log in to the FTP Server with the User Name and Password Provided */
                ftpRequest.Credentials = new NetworkCredential(user, pass);
                /* When in doubt, use these options */
                ftpRequest.UseBinary = true;
                ftpRequest.UsePassive = true;
                ftpRequest.KeepAlive = true;
                /* Specify the Type of FTP Request */
                ftpRequest.Method = WebRequestMethods.Ftp.ListDirectoryDetails;
                /* Establish Return Communication with the FTP Server */
                ftpResponse = (FtpWebResponse)ftpRequest.GetResponse();
                /* Establish Return Communication with the FTP Server */
                ftpStream = ftpResponse.GetResponseStream();
                /* Get the FTP Server's Response Stream */
                StreamReader ftpReader = new StreamReader(ftpStream);
                /* Store the Raw Response */
                string directoryRaw = null;
                /* Read Each Line of the Response and Append a Pipe to Each Line for Easy Parsing */
                try { while (ftpReader.Peek() != -1) { directoryRaw += ftpReader.ReadLine() + "|"; } }
                catch (Exception ex) { Console.WriteLine(ex.ToString()); }
                /* Resource Cleanup */
                ftpReader.Close();
                ftpStream.Close();
                ftpResponse.Close();
                ftpRequest = null;
                /* Return the Directory Listing as a string Array by Parsing 'directoryRaw' with the Delimiter you Append (I use | in This Example) */
                try { string[] directoryList = directoryRaw.Split("|".ToCharArray()); return directoryList; }
                catch (Exception ex) { Console.WriteLine(ex.ToString()); }
            }
            catch (Exception ex) { Console.WriteLine(ex.ToString()); }
            /* Return an Empty string Array if an Exception Occurs */
            return new string[] { "" };
        }
    }
}
