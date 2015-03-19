using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Reflection;
using System.Xml.Linq;
using System.Data;
using System.Data.SqlClient;
using System.Configuration; 


namespace cop483assgn234.AppCode
{
    public interface IXmlPropertyObject
    {
        // Required Properties
        int id { get; set; }
        string Name { get; set; }
        string Description { get; set; }
        string Owner { get; set; }
        // Required Methods
        string asXML();
        string className();
        string getPropertyFromXml(string property, string xml);
        string getPropertyValue(string name);
        void setID(int ID);
        void setPropertyFromXml(string property, string xml);
        void setProperty(string name, string value);
        void setPropertiesFromXml(string xml);
    }

    public class XmlPropertyObject : IXmlPropertyObject
    {
        public int id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public string Owner { get; set; }
        public XmlPropertyObject(int ID)
        {
            id = ID;
            Select(ID);
        }
        public XmlPropertyObject() { }
        public string asXML()
        {
            string s = String.Empty;
            string fc = className();
            while (fc.Contains("."))
            {
                fc = fc.Substring(fc.IndexOf(".") + 1);
            }
            s += "<" + fc + ">\n";
            foreach (var property in this.GetType().GetProperties())
            {
                s += " <" + property.Name + ">";
                s += Convert.ToString(property.GetValue(this));
                s += "</" + property.Name + ">";
                s += "\n";
            }
            s += "</" + fc + ">";
            return s;
        }
        public string className()
        {
            string fc = this.GetType().ToString();
            while (fc.Contains("."))
            {
                fc = fc.Substring(fc.IndexOf(".") + 1);
            }
            return fc;
        }
        public string getPropertyFromXml(string property, string xml)
        {
            XDocument doc = XDocument.Parse(xml);
            // Gets the value fom XML
            try { return doc.Root.Element(property).Value; }
            catch { return String.Empty; }
        }
        public void setID(int ID)
        {
            id = ID;
            Select(ID);
        }
        public void setPropertyFromXml(string property, string xml)
        {
            string value = getPropertyFromXml(property, xml);
            setProperty(property, value);
        }
        public string getPropertyValue(string name)
        {
            PropertyInfo pi = this.GetType().GetProperty(name, BindingFlags.Public | BindingFlags.Instance);
            return Convert.ToString(pi.GetValue(this));
        }
        public void setProperty(string name, string value)
        {
            PropertyInfo pi = this.GetType().GetProperty(name, BindingFlags.Public | BindingFlags.Instance);
            if (null != pi && pi.CanWrite)
            {
                pi.SetValue(this, Convert.ChangeType(value, pi.PropertyType), null);
            }
        }
        public void setPropertiesFromXml(string xml)
        {
            XDocument doc = XDocument.Parse(xml);
            foreach (XElement xe in doc.Root.Elements())
            {
                setProperty(xe.Name.ToString(), xe.Value);
            }
        }
        public void Select(int ID)
        {
            DataTable dt = DatabaseXmlStore.Select(ID);
            if (dt.Rows.Count == 0) return;
            setPropertiesFromXml(dt.Rows[0]["XmlData"].ToString());
            id = ID;
        }
        public DataTable SelectAll(string ownerID)
        {
            return DatabaseXmlStore.SelectAll(ownerID, className());
        }
        public string Insert()
        {
            return DatabaseXmlStore.Insert(this);
        }
        public string Update()
        {
            if (id == 0)
                return Insert();
            else
                return DatabaseXmlStore.Update(this);
        }
    }
    public class DatabaseXmlStore
    {
        // Allows for the storage and retrieval of objects from the database
        // That conform to IXmlPropertyObject 
        /*
         * 
         CREATE TABLE XmlObject (
          id INT PRIMARY KEY IDENTITY(1,1),
          Class VARCHAR(255) NOT NULL,
          Name VARCHAR(255) NULL,
          Description VARCHAR(500) NULL,
          XMLData VARCHAR(MAX) NULL,
          OwnerUserID VARCHAR(255) NOT NULL,
          LastEditedDate DATETIME)
                  * * 
         */
        // Properties
        public static SqlConnection Connection()
        {
            // This should be read from the Web.config
            string cs = System.Configuration.ConfigurationManager.ConnectionStrings["ObjectDatabase"].ConnectionString;
            return new SqlConnection(cs);
        }
        #region Insert
        public static string Insert(IXmlPropertyObject o)
        {
            string objectName = o.className();
            string sql = "INSERT INTO XmlObject ";
            sql += "(Class, Name, Description, XMLData, OwnerUserID, LastEditedDate)";
            sql += "VALUES";
            sql += "(@Class, @Name, @Description, @XMLData, @OwnerUserID, GETDATE())";
            SqlConnection connection = DatabaseXmlStore.Connection();
            using (connection)
            {
                SqlCommand command = new SqlCommand(sql, connection);
                command.Parameters.Add("@Class", SqlDbType.VarChar);
                command.Parameters["@Class"].Value = o.className();
                command.Parameters.Add("@Name", SqlDbType.VarChar);
                command.Parameters["@Name"].Value = o.Name;
                command.Parameters.Add("@Description", SqlDbType.VarChar);
                command.Parameters["@Description"].Value = o.Description;
                command.Parameters.Add("@XmlData", SqlDbType.VarChar);
                command.Parameters["@XmlData"].Value = o.asXML();
                command.Parameters.Add("@OwnerUserID", SqlDbType.VarChar);
                command.Parameters["@OwnerUserID"].Value = o.Owner;
                try
                {
                    connection.Open();
                    Int32 rowsAffected = command.ExecuteNonQuery();
                    return "Data Inserted";
                }
                catch (Exception ex)
                {
                    return ex.Message;
                }
            }
        }
        #endregion
        #region Select
        public static DataTable Select(int id)
        {
            string sql = "SELECT * FROM XmlObject ";
            sql += "WHERE id = @ID ";
            SqlConnection connection = DatabaseXmlStore.Connection();
            using (connection)
            {
                SqlCommand command = new SqlCommand(sql, connection);
                command.Parameters.Add("@ID", SqlDbType.Int);
                command.Parameters["@ID"].Value = id;
                try
                {
                    DataTable dt = new DataTable();
                    connection.Open();
                    SqlDataAdapter sda = new SqlDataAdapter(command);
                    sda.Fill(dt);
                    return dt;
                }
                catch
                {
                    return new DataTable();
                }
            }
        }
        public static DataTable SelectAll(string ownerID)
        {
            string sql = "SELECT * FROM XmlObject ";
            sql += "WHERE OwnerUserId = @OwnerUserID ";
            SqlConnection connection = DatabaseXmlStore.Connection();
            using (connection)
            {
                SqlCommand command = new SqlCommand(sql, connection);
                command.Parameters.Add("@OwnerUserID", SqlDbType.VarChar);
                command.Parameters["@OwnerUserID"].Value = ownerID;
                try
                {
                    DataTable dt = new DataTable();
                    connection.Open();
                    SqlDataAdapter sda = new SqlDataAdapter(command);
                    sda.Fill(dt);
                    return dt;
                }
                catch
                {
                    return new DataTable();
                }
            }
        }
        public static DataTable SelectAll(string ownerID, string objectClass)
        {
            string sql = "SELECT * FROM XmlObject ";
            sql += "WHERE OwnerUserId = @OwnerUserID ";
            sql += "AND Class = @Class";
            SqlConnection connection = DatabaseXmlStore.Connection();
            using (connection)
            {
                SqlCommand command = new SqlCommand(sql, connection);
                command.Parameters.Add("@Class", SqlDbType.VarChar);
                command.Parameters["@Class"].Value = objectClass;
                command.Parameters.Add("@OwnerUserID", SqlDbType.VarChar);
                command.Parameters["@OwnerUserID"].Value = ownerID;
                try
                {
                    DataTable dt = new DataTable();
                    connection.Open();
                    SqlDataAdapter sda = new SqlDataAdapter(command);
                    sda.Fill(dt);
                    return dt;
                }
                catch
                {
                    return new DataTable();
                }
            }
        }
        #endregion
        public static string Delete(IXmlPropertyObject o)
        {
            string sql = "DELETE FROM XmlObject ";
            sql += "WHERE id = @id ";
            SqlConnection connection = DatabaseXmlStore.Connection();
            using (connection)
            {
                SqlCommand command = new SqlCommand(sql, connection);
                command.Parameters.Add("@id", SqlDbType.Int);
                command.Parameters["@id"].Value = o.id;
                try
                {
                    connection.Open();
                    command.ExecuteNonQuery();
                    return "Object Deleted";
                }
                catch (Exception ex)
                {
                    return ex.Message;
                }
            }
        } // End Delete
        public static string Update(IXmlPropertyObject o)
        {
            string objectName = o.className();
            string sql = "UPDATE XmlObject ";
            sql += "SET Class = @Class, ";
            sql += "Name = @Name, ";
            sql += "Description = @Description, ";
            sql += "XMLData = @XmlData, ";
            sql += "OwnerUserID = @OwnerUserID, ";
            sql += "LastEditedDate = GETDATE()";
            sql += "WHERE id = @id";
            SqlConnection connection = DatabaseXmlStore.Connection();
            using (connection)
            {
                SqlCommand command = new SqlCommand(sql, connection);
                command.Parameters.Add("@Class", SqlDbType.VarChar);
                command.Parameters["@Class"].Value = o.className();
                command.Parameters.Add("@Name", SqlDbType.VarChar);
                command.Parameters["@Name"].Value = o.Name;
                command.Parameters.Add("@Description", SqlDbType.VarChar);
                command.Parameters["@Description"].Value = o.Description;
                command.Parameters.Add("@XmlData", SqlDbType.VarChar);
                command.Parameters["@XmlData"].Value = o.asXML();
                command.Parameters.Add("@OwnerUserID", SqlDbType.VarChar);
                command.Parameters["@OwnerUserID"].Value = o.Owner;
                command.Parameters.Add("@id", SqlDbType.Int);
                command.Parameters["@id"].Value = o.id;
                try
                {
                    connection.Open();
                    Int32 rowsAffected = command.ExecuteNonQuery();
                    return "Data Updated";
                }
                catch (Exception ex)
                {
                    return ex.Message;
                }
            }
        } // End Update
    } // End Class 

    public class SamplePropertiesInherited : XmlPropertyObject
    {
        public string Content { get; set; }
    }

}