using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Xml;
using System.Xml.Serialization;

namespace DFBProject
{
	public class Project
	{
		public string Version { get; set; }
		public string ProjectFileName { get; set; }
		public string Bus1Data { get; set; }
		public string Bus2Data { get; set; }
		public string Code { get; set; }
		public int CyclesToRun { get; set; }
		public List<InputSequence> InputSequence { get; set; }

		public Project()
		{
			InputSequence = new List<InputSequence>();
			Version = Assembly.GetExecutingAssembly().GetName().Version.ToString();
		}

		public void Save()
		{
			SerializeObject(this, ProjectFileName);
		}

		public void Open(string projectFileName)
		{
			if (File.Exists(projectFileName))
			{
				var temp = DeSerializeObject<Project>(projectFileName);
				UpgradeCheck();

				this.Version = temp.Version;
				this.ProjectFileName = temp.ProjectFileName;
				this.Bus1Data = temp.Bus1Data;
				this.Bus2Data = temp.Bus2Data;
				this.Code = temp.Code;
				this.InputSequence = temp.InputSequence;
				this.CyclesToRun = temp.CyclesToRun;

				if (this.InputSequence == null)
				{
					this.InputSequence = new List<InputSequence>();
				}
			}
		}

		/// <summary>
		/// Serializes an object.
		/// </summary>
		/// <typeparam name="T"></typeparam>
		/// <param name="serializableObject"></param>
		/// <param name="fileName"></param>
		private void SerializeObject<T>(T serializableObject, string fileName)
		{
			if (serializableObject == null) { return; }

			XmlDocument xmlDocument = new XmlDocument();
			XmlSerializer serializer = new XmlSerializer(serializableObject.GetType());
			using (MemoryStream stream = new MemoryStream())
			{
				serializer.Serialize(stream, serializableObject);
				stream.Position = 0;
				xmlDocument.Load(stream);
				xmlDocument.Save(fileName);
				stream.Close();
			}
		}

		/// <summary>
		/// Deserializes an xml file into an object list
		/// </summary>
		/// <typeparam name="T"></typeparam>
		/// <param name="fileName"></param>
		/// <returns></returns>
		private T DeSerializeObject<T>(string fileName)
		{
			if (string.IsNullOrEmpty(fileName)) { return default(T); }

			T objectOut = default(T);

			XmlDocument xmlDocument = new XmlDocument();
			xmlDocument.Load(fileName);
			string xmlString = xmlDocument.OuterXml;

			using (StringReader read = new StringReader(xmlString))
			{
				Type outType = typeof(T);

				XmlSerializer serializer = new XmlSerializer(outType);
				using (XmlReader reader = new XmlTextReader(read))
				{
					objectOut = (T)serializer.Deserialize(reader);
					reader.Close();
				}

				read.Close();
			}

			return objectOut;
		}

		private void UpgradeCheck()
		{
			var codeVersion = Assembly.GetExecutingAssembly().GetName().Version.ToString();
			if (!this.Version.Equals(codeVersion))
			{
				// ToDo: add file upgrade routine here if ever needed
			}
		}
	}
}
