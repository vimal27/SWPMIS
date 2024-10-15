/// <summary>
/// Required Namespaces
/// </summary>
using System;
using System.Data;
using System.Configuration;
using System.Collections;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Web.UI.HtmlControls;
using System.Data.SqlClient;
using System.IO;
using System.Data.OleDb;
using System.Net;
using System.Globalization;
using System.Text;

public partial class in_projectanalysis : System.Web.UI.Page
{
	/// <summary>
	/// Declarations Part For Variables,Strings,SqlConnections,etc
	/// </summary>
    String var_Filename;
    string ProjectAlloted;
    String var_Date, var_Month, var_DateOnly, var_Year, var_Hour, var_Minute, var_Second, var_SubPath, var_ProjectName, var_UpdatedOn;
    SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["Conn"].ToString());
    SqlConnection con2 = new SqlConnection(ConfigurationManager.ConnectionStrings["Conn"].ToString());
    SqlCommand com = new SqlCommand();
    SqlCommand com2 = new SqlCommand();
    SqlCommand com_Delete = new SqlCommand();
    SqlCommand com_check = new SqlCommand();
    DataSet ds = new DataSet();

	/// <summary>
	/// Page_s the load.
	/// </summary>
	/// <param name="sender">Sender.</param>
	/// <param name="e">E.</param>
    protected void Page_Load(object sender, EventArgs e)
    {
        try
        {
			//Block Direct URL Access
            if (Request.UrlReferrer == null)
            {
                Response.Redirect("Login");
            }
            else
            {
                ddl_ProjectName.Focus();
				//Redirect to Login page If Session is Expired
                if (Session["Userid"] == null) Response.Redirect("Login");
                else if (Session["Userid"].ToString() == "") Response.Redirect("Login");
               
                if (!Page.IsPostBack)
                {
                    ViewState["userid"] = Convert.ToString(Session["Userid"]);
                    bindGrid();
                    con.Open();
                    com2.Connection = con;
                    string Rights = Convert.ToString(Session["Rights"]);
                    if (Rights == "Team Leader")
                    {
                        con2.Close();
                        con2.Open();
                        com_check.Connection = con2;
                        com_check.CommandText = "select TeamID from tbl_teamAllotmentMaster where teamleader like  ('%" + Session["Userid"] + "%')";
                        SqlDataReader dr = com_check.ExecuteReader();
                        if (dr.HasRows)
                        {
                            StringBuilder sb_ProjectAlloted = new StringBuilder();
                            while (dr.Read())
                            {
                                sb_ProjectAlloted.Append(dr["TeamID"] + ",");
                                ProjectAlloted = sb_ProjectAlloted.ToString();
                            }
                            ProjectAlloted = ProjectAlloted.TrimEnd(',');
                        }
                        dr.Close();
                        con2.Close();
                        string[] SplittedProjectAlloted = ProjectAlloted.Split(',');
                        com2.CommandText = "Select projectid,projectreq from tbl_projectReq where projectstatus in('WIP','Yet To Start') and (";
                        foreach (string Splitted in SplittedProjectAlloted)
                        {
                            com2.CommandText += "allotedteamid like '%" + Splitted + "%' or ";
                        }
                        com2.CommandText = com2.CommandText.Substring(0, com2.CommandText.Length - 3);
                        com2.CommandText += ")";
                    }
                    else
                    {
                        com2.CommandText = "Select projectid,projectreq from tbl_projectReq where status=1";
                    }
                    ddl_ProjectName.DataSource = com2.ExecuteReader();
                    ddl_ProjectName.Items.Clear();
                    ddl_ProjectName.DataTextField = "projectreq";
                    ddl_ProjectName.DataValueField = "projectid";
                    ddl_ProjectName.DataBind();
                    ddl_ProjectName.Items.Insert(0, new ListItem("Select", "NA"));
                    con.Close();
                    Page.Form.Attributes.Add("enctype", "multipart/form-data");
                }
            }
        }
        catch (Exception)
        {
        }
    }

	/// <summary>
	/// Binds the gridview.
	/// </summary>
    void bindGrid()
    {
        try
        {
			SqlDataAdapter da = new SqlDataAdapter("select sno,sno[Revision no],Filenaming[File Name],updatedon[Updated On] from tbl_projectAnalysis where projectid='" + ddl_ProjectName.SelectedValue.ToString().Trim() + "'", con);

            DataSet ds = new DataSet();
            da.Fill(ds);
            if (ds.Tables[0].Rows.Count > 0)
            {
                grd_Projects.DataSource = ds;
                grd_Projects.DataBind();
            }
            else
            {
                ds.Tables[0].Rows.Add(ds.Tables[0].NewRow());
                grd_Projects.DataSource = ds;
                grd_Projects.DataBind();
                int columncount = grd_Projects.Rows[0].Cells.Count;
                grd_Projects.Rows[0].Cells.Clear();
                grd_Projects.Rows[0].Cells.Add(new TableCell());
                grd_Projects.Rows[0].Cells[0].ColumnSpan = columncount;
                grd_Projects.Rows[0].Cells[0].Text = "<center>----- No Records Found -----</center>";
            }
        }
        catch (Exception)
        {
        }
    }

	/// <summary>
	/// Projects Delete Function
	/// </summary>
	/// <param name="sender">Sender.</param>
	/// <param name="e">E.</param>
    protected void grd_Projects_RowDeleting(object sender, GridViewDeleteEventArgs e)
    {
        try
        {
            string confirmValue = Request.Form["confirm_value"];
            if (confirmValue == "Yes")
            {
                int id = (int)grd_Projects.DataKeys[e.RowIndex].Value;
                con.Open();
                com_Delete.Connection = con;
                con.Close();
                con.Open();
                com_Delete.CommandText = "select FilePath+'/'+tempname as FileToDelete from tbl_projectanalysis where sno='" + id + "'";
                SqlDataReader dr = com_Delete.ExecuteReader();
                if (dr.HasRows)
                {
                    dr.Read();
                    string FileToDelete = dr[0].ToString();
                    string deleteFile = Server.MapPath(FileToDelete);
                    if (FileToDelete != null || FileToDelete != string.Empty)
                    {
                        if ((System.IO.File.Exists(deleteFile)))
                            System.IO.File.Delete(deleteFile);
                    }
                }
                dr.Close();
                com_Delete.Connection = con;
                com_Delete.CommandText = "delete from tbl_projectanalysis where sno='" + id + "'";
                com_Delete.ExecuteNonQuery();
                bindGrid();

            }
            else
            {

            }
        }
        catch (Exception)
        {
        }
    }

	/// <summary>
	/// Uploads the Excel file.
	/// </summary>
	/// <param name="sender">Sender.</param>
	/// <param name="e">E.</param>
    protected void UploadFile(object sender, EventArgs e)
    {
        try
        {
            if (ddl_ProjectName.SelectedItem.Value == "NA")
            {
                ScriptManager.RegisterStartupScript(this,GetType(),"mismatch","swal('Please choose Project details');", true);
            }
            else
            {
                var_ProjectName = ddl_ProjectName.SelectedItem.Text;
                var_Year = DateTime.Now.ToString("yyyy");
                var_Month = DateTime.Now.ToString("MMM");
                string var_MonthInt = DateTime.Now.ToString("MM");
                var_DateOnly = DateTime.Now.ToString("dd");
                var_Date = var_Month + "-" + DateTime.Now.ToString("dd");
                var_Hour = DateTime.Now.ToString("hh");
                var_Minute = DateTime.Now.ToString("mm");
                var_Second = DateTime.Now.ToString("ss" + " " + "tt");
                var_UpdatedOn = var_DateOnly + "/" + var_MonthInt + "/" + var_Year + " " + var_Hour + ":" + var_Minute + ":" + var_Second;
                var_SubPath = "Files/" + var_ProjectName.Trim() + "/" + var_Year + "/" + var_Month + "/" + var_Date;
                bool exists = System.IO.Directory.Exists(Server.MapPath(var_SubPath));
                if (!exists)
                {
                    System.IO.Directory.CreateDirectory(Server.MapPath(var_SubPath));
                }
                if (upl_File.HasFile)
                {
                    con.Open();
                    string var_Tempname = Path.GetFileName(upl_File.PostedFile.FileName);
                    string var_Extension = Path.GetExtension(upl_File.PostedFile.FileName);
                    var_Filename = var_Hour + "-" + var_Minute + "-" + var_Second + var_Extension;
                    string var_FolderPath = Server.MapPath(var_SubPath);
                    string var_FilePath = Server.MapPath(var_SubPath + "/" + var_Filename);

                    upl_File.SaveAs(var_FilePath);
                    com.Connection = con;
                    com.CommandText = "insert into tbl_projectAnalysis (projectid,projectname,Filenaming,FilePath,tempname,updatedon) values ('" + hdn_ProjectID_Selected.Value + "','" + hdn_ProjectNameSelected.Value + "','" + var_Tempname + "','" + var_SubPath + "','" + var_Filename + "','" + var_UpdatedOn + "')";
                    com.ExecuteNonQuery();
                    bindGrid();
                    hdn_FilePath.Value = var_FilePath;
                    hdn_Extension.Value = var_Extension;
                    //Import_To_Grid(FilePath, Extension);
                }
                else
                {
                    ScriptManager.RegisterStartupScript(this,GetType(),"mismatch","swal('Please Choose File To Upload');", true);
                }
            }
        }
        catch (Exception)
        {
        }
    }
    void clearData()
    {


    }

    void binddata()
    {



    }



    protected void Grid_Change(Object sender, DataGridPageChangedEventArgs e)
    {

    }

    protected void dataGrid1_SelectedIndexChanged(object sender, EventArgs e)
    {

    }

    protected void cmdcancel_Click(object sender, ImageClickEventArgs e)
    {
        try
        {
            clearData();
        }
        catch (Exception)
        {

        }
    }
    protected void dataGrid1_ItemCommand(object source, DataGridCommandEventArgs e)
    {
        if (e.CommandName == "Deletee")
        {

        }
    }
    protected void LinkButton1_Click(object sender, EventArgs e)
    {

    }

	/// <summary>
	/// Projects Update when selected project on gridview
	/// </summary>
	/// <param name="sender">Sender.</param>
	/// <param name="e">E.</param>
    protected void grd_Projects_SelectedIndexChanged(object sender, EventArgs e)
    {
        try
        {
            GridViewRow row = grd_Projects.SelectedRow;
            string var_FolderPath = ConfigurationManager.AppSettings["FolderPath"];
            con.Open();
            SqlDataAdapter da = new SqlDataAdapter("select Filenaming from tbl_projectAnalysis where sno='" + row.Cells[1].Text + "'", con);

            string var_FileName = row.Cells[2].Text;
            string var_Path = var_FolderPath + var_FileName;
            string var_FilePath = Server.MapPath(var_FolderPath + var_FileName);
            FileInfo var_Info = new FileInfo(Server.MapPath(var_Path));
            if (var_Info.Exists)
            {
            }
            else
            {
                Response.Write("This file does not exist.");
            }
        }
        catch (Exception)
        {
        }
    }

	/// <summary>
	/// Currently Not In Use
	/// </summary>
	/// <param name="sender">Sender.</param>
	/// <param name="e">E.</param>
    protected void OnPageIndexChanging(object sender, GridViewPageEventArgs e)
    {
        try
        {
            grd_Projects.PageIndex = e.NewPageIndex;
            this.bindGrid();
        }
        catch (Exception)
        {
        }
    }

	/// <summary>
	/// Projectname Dropdownlist Changed event To show records of selected project
	/// </summary>
	/// <param name="sender">Sender.</param>
	/// <param name="e">E.</param>
    protected void ddl_ProjectName_SelectedIndexChanged(object sender, EventArgs e)
    {
        try
        {
            bindGrid();
            if (ddl_ProjectName.SelectedIndex != 0)
            {
                lbl_ProjectID_Name.Text = ddl_ProjectName.SelectedItem.Text;
                hdn_ProjectID_Selected.Value = ddl_ProjectName.SelectedValue;
                hdn_ProjectNameSelected.Value = ddl_ProjectName.SelectedItem.Text;
            }
            else
            {
                lbl_ProjectID_Name.Text = "";
            }
        }
        catch (Exception)
        {
        }
    }
    protected void grd_Projects_ItemCommand()
    {

    }

	/// <summary>
	/// Download selected project documents
	/// </summary>
	/// <param name="sender">Sender.</param>
	/// <param name="e">E.</param>
    protected void lnk_Download_Click(object sender, EventArgs e)
    {

        try
        {
            LinkButton lk_Button = sender as LinkButton;
            GridViewRow grd_Row = lk_Button.NamingContainer as GridViewRow;
            string var_Sno = grd_Projects.DataKeys[grd_Row.RowIndex].Value.ToString();
            SqlCommand cmd = new SqlCommand("select tempname,FilePath,Filenaming from tbl_projectAnalysis where sno='" + var_Sno + "'", con);
            if (con.State == ConnectionState.Closed) con.Open();
            SqlDataReader dr = cmd.ExecuteReader();
            dr.Read();
            {
                if (dr.HasRows)
                {

                    String var_FileName = dr["tempname"].ToString();
                    String var_FilePath = dr.GetString(1);
                    String var_SaveAs = dr.GetString(2);
                    if (var_Filename != "")
                    {

                        string filePath = Server.MapPath(".").Trim() + "\\" + var_FilePath + "\\" + var_FileName;
                        //string pathfile = Server.MapPath(".").Trim() + " \\" + subpath + "\\";
                        if (File.Exists(filePath))
                        {
                            var file = new System.IO.FileInfo(filePath);
                            Response.Clear();
                            Response.AddHeader("Content-Disposition", "attachment;filename=\"" + var_SaveAs + "\"");
                            Response.AddHeader("Content-Length", file.Length.ToString(CultureInfo.InvariantCulture));
                            Response.ContentType = "application/octet-stream";
                            Response.Flush();
                            Response.TransmitFile(filePath);
                            Response.End();

                        }
                        else
                        {
                            ScriptManager.RegisterStartupScript(this,GetType(),"mismatch","swal('Selected File Was Not Found On Server');", true);
                        }
                    }
                }
            }



        }
        catch (Exception)
        {
        }

    }

	/// <summary>
	/// Redirect to dashboard If Back button is Clicked
	/// </summary>
	/// <param name="sender">Sender.</param>
	/// <param name="e">E.</param>
    protected void btn_Back_Click(object sender, EventArgs e)
    {
        Response.Redirect("Dashboard");
    }
}