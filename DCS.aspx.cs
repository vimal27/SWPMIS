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
using System.Diagnostics;
using System.Text;
using System.Collections.Generic;
using System.Drawing;
using System.Globalization;


public partial class DCS : System.Web.UI.Page
{
    /// <summary>
    /// Declarations Part For Variables,Strings,SqlConnections,etc
    /// </summary>
    SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["Conn"].ToString());
    SqlConnection con2 = new SqlConnection(ConfigurationManager.ConnectionStrings["Conn"].ToString());
    SqlCommand com_Check = new SqlCommand();
    SqlCommand com_DCS = new SqlCommand();
    String ProjectAlloted;
    clsDataControl objData = new clsDataControl();

    /// <summary>
    /// Page_s the load.
    /// </summary>
    /// <param name="sender">Sender.</param>
    /// <param name="e">E.</param>
    protected void Page_Load(object sender, EventArgs e)
    {
        try
        {
            if (Request.UrlReferrer == null)
            {
                Response.Redirect("Login");

            }
            txt_ApprovedBy.Attributes.Add("readonly", "readonly");
            txt_ReceivedDate.Attributes.Add("readonly", "readonly");
            txt_DispatchDate.Attributes.Add("readonly", "readonly");
            txt_DepartmentName.Text = "SOFTWARE";
            txt_DepartmentName.Attributes.Add("readonly", "readonly");
            txt_ApprovedBy.Text = "Saravanan T";
            if (!IsPostBack)
            {
                //Set Approved by to TL
                //binding Gridview
                dcs_gridbind();
                //To bind Projects For ddl_ProjectIDs
                bindProjects();
                //Required Field Validations
                btn_Save.Attributes.Add("onclick", "return validate()");
            }
        }
        catch (Exception)
        {
        }
    }
    /// <summary>
    /// Bind Projects For ddl_ProjectIDs
    /// </summary>
    protected void bindProjects()
    {
        try
        {
            string Rights = Convert.ToString(Session["Rights"]);
            ddl_ProjectID.Items.Clear();
            con.Close();
            con.Open();

            SqlDataAdapter sda = new SqlDataAdapter("Select projectid,projectname,projectreq,manualid,manualid+'-'+projectname as name  from tbl_projectReq where projectstatus!='Hold' and projectstatus!='Closed' and status!=0 and typeproject='External'", con);
            if (Rights == "Team Leader")
            {
                con2.Close();
                con2.Open();
                com_Check.Connection = con2;
                com_Check.CommandText = "select TeamID from tbl_teamAllotmentMaster where teamleader like  ('%" + Session["Userid"] + "%')";
                SqlDataReader dr = com_Check.ExecuteReader();
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
                com_Check.Connection = con;
                com_Check.CommandText = "Select sno,projectid,projectname,projectreq,manualid,manualid+'-'+projectname as name from tbl_projectReq where projectstatus!='Hold' and projectstatus!='Closed'  and status!=0 and typeproject='External' and (";
                foreach (string Splitted in SplittedProjectAlloted)
                {
                    com_Check.CommandText += "allotedteamid like '%" + Splitted + "%' or ";
                }
                com_Check.CommandText = com_Check.CommandText.Substring(0, com_Check.CommandText.Length - 3);
                com_Check.CommandText += ")";
                sda = new SqlDataAdapter(com_Check.CommandText, con);
            }
            DataSet ds = new DataSet();
            sda.Fill(ds);
            ddl_ProjectID.DataSource = ds;
            ddl_ProjectID.DataTextField = "name";
            ddl_ProjectID.DataValueField = "manualid";
            ddl_ProjectID.DataBind();
            con.Close();
        }
        catch (Exception)
        {


        }

        ddl_ProjectID.Items.Insert(0, new ListItem("Select", "N/A"));
    }
    /// <summary>
    /// Change text for project Name label based on selected project ID
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void ddl_ProjectID_SelectedIndexChanged(object sender, EventArgs e)
    {
        String SelectedProjectName = objData.GetSingleData("select projectname from tbl_ProjectReq where manualid='" + ddl_ProjectID.SelectedValue + "'");
        if (SelectedProjectName != "0")
        {
            lbl_ProjectNameView.Text = SelectedProjectName;
        }
        string teamLeader = objData.GetSingleData("select username from tbl_usermaster where userid in(select teamleader from tbl_teamAllotmentMaster where teamid in(select allotedteamid from tbl_ProjectReq where manualid='" + ddl_ProjectID.SelectedValue + "'))");
        bindDropDowns();
        if (ddl_ProjectID.SelectedIndex != 0 && ddl_ProjectID.SelectedValue != "NA")
        {
            generateOrderNumber();
        }
        else
        {
            lbl_OrderNO_View.Text = "NA";
            lbl_ProjectNameView.Text = "NA";
            lbl_SlnoView.Value = "";
        }
        dcs_gridbind();
    }
    /// <summary>
    /// Bind Activity1 to Activity6 Dropdownlists
    /// </summary>
    protected void bindDropDowns()
    {
        try
        {
            DataTable dt_CSRNames = objData.Getdata("select 'CSR - '+convert(varchar(10),id) [CSRID],'CSR - '+name[CSRName] from tbl_Coordinator where projectid='" + ddl_ProjectID.SelectedValue + "'");
            Dictionary<string, string> dct_TeamLeaders = new Dictionary<string, string>();
            string teamidSelected = objData.GetSingleData("select ''''+replace(allotedteamid,',',''',''')+'''' from tbl_ProjectReq where manualid='" + ddl_ProjectID.SelectedValue + "'");
            string usersids = objData.GetSingleData("select ''''+replace(userid,',',''',''')+'''' from tbl_teamAllotmentMaster where teamid in(" + teamidSelected + ")");
            //string usernames = objData.GetSingleData("select ''''+replace(username,',',''',''')+'''' from tbl_teamAllotmentMaster where teamid ='" + teamidSelected + "'");
            ddl_Activity1.Items.Clear();
            ddl_Activity2.Items.Clear();
            ddl_Activity3.Items.Clear();
            ddl_Activity4.Items.Clear();
            ddl_Activity5.Items.Clear();
            ddl_Activity6.Items.Clear();
            ddl_CheckedBy.Items.Clear();
            con.Close();
            con.Open();
            DataTable dt_TeamLeaders = objData.Getdata("select teamleader,(select b.username from tbl_usermaster b where b.userid=teamleader)[teamleadername] from tbl_teamAllotmentMaster where teamid in(" + teamidSelected + ")");
            for (int i = 0; i < dt_TeamLeaders.Rows.Count; i++)
            {
                dct_TeamLeaders.Add(dt_TeamLeaders.Rows[i]["teamleader"].ToString(), dt_TeamLeaders.Rows[i]["teamleadername"].ToString());
            }
            ddl_CheckedBy.DataSource = dct_TeamLeaders;
            ddl_CheckedBy.DataTextField = "Value";
            ddl_CheckedBy.DataValueField = "Key";
            ddl_CheckedBy.DataBind();
            SqlCommand cmd = new SqlCommand("select userid,username from tbl_teamAllotmentMaster where teamid in(" + teamidSelected + ")", con);
            SqlDataReader dr = cmd.ExecuteReader();
            dr.Read();
            if (dr.HasRows)
            {
                string teamid = dr["userid"].ToString();
                string teamname = dr["username"].ToString();
                string[] splittedteamid = teamid.Split(',');
                string[] splittedteamname = teamname.Split(',');
                Dictionary<string, string> teams = new Dictionary<string, string>();

                for (int i = 0; i < splittedteamid.Length; i++)
                {
                    teams.Add(splittedteamid[i], splittedteamname[i]);
                    ddl_Activity1.DataSource = teams;
                    ddl_Activity1.DataTextField = "Value";
                    ddl_Activity1.DataValueField = "Key";
                    ddl_Activity1.DataBind();
                    ddl_Activity1.Items.Insert(0, new ListItem("Saravanan T", "L1040"));
                    ddl_Activity2.DataSource = teams;
                    ddl_Activity2.DataTextField = "Value";
                    ddl_Activity2.DataValueField = "Key";
                    ddl_Activity2.DataBind();
                    ddl_Activity2.Items.Insert(0, new ListItem("Saravanan T", "L1040"));
                    ddl_Activity3.DataSource = teams;
                    ddl_Activity3.DataTextField = "Value";
                    ddl_Activity3.DataValueField = "Key";
                    ddl_Activity3.DataBind();
                    ddl_Activity3.Items.Insert(0, new ListItem("Saravanan T", "L1040"));
                    ddl_Activity4.DataSource = teams;
                    ddl_Activity4.DataTextField = "Value";
                    ddl_Activity4.DataValueField = "Key";
                    ddl_Activity4.DataBind();
                    ddl_Activity4.Items.Insert(0, new ListItem("Saravanan T", "L1040"));

                    ddl_Activity5.DataTextField = "CSRName";
                    ddl_Activity5.DataValueField = "CSRID";
                    ddl_Activity5.DataSource = dt_CSRNames;
                    ddl_Activity5.DataBind();
                    ddl_Activity5.Items.Insert(0, new ListItem("Saravanan T", "L1040"));
                    ddl_Activity6.DataTextField = "CSRName";
                    ddl_Activity6.DataValueField = "CSRID";
                    ddl_Activity6.DataSource = dt_CSRNames;
                    ddl_Activity6.DataBind();
                    ddl_Activity6.Items.Insert(0, new ListItem("Saravanan T", "L1040"));
                    //ddl_Activity5.DataSource = teams;
                    //ddl_Activity5.DataTextField = "Value";
                    //ddl_Activity5.DataValueField = "Key";
                    //ddl_Activity5.DataBind();
                    //ddl_Activity6.DataSource = teams;
                    //ddl_Activity6.DataTextField = "Value";
                    //ddl_Activity6.DataValueField = "Key";
                    //ddl_Activity6.DataBind();
                }
            }
            dr.Close();
            con.Close();
            ddl_CheckedBy.Items.Insert(0, new ListItem("Select", "N/A"));
            ddl_Activity1.Items.Insert(0, new ListItem("Select", "N/A"));
            ddl_Activity2.Items.Insert(0, new ListItem("Select", "N/A"));
            ddl_Activity3.Items.Insert(0, new ListItem("Select", "N/A"));
            ddl_Activity4.Items.Insert(0, new ListItem("Select", "N/A"));
            ddl_Activity5.Items.Insert(0, new ListItem("Select", "N/A"));
            ddl_Activity6.Items.Insert(0, new ListItem("Select", "N/A"));
        }
        catch (Exception ex)
        {


        }
    }
    /// <summary>
    /// Generate Unique Order Number
    /// </summary>
    protected void generateOrderNumber()
    {
        string curYear = DateTime.Today.Year.ToString();
        string curMonth = DateTime.Today.Month.ToString();
        string Orderno = "DC";
        //Orderno += ddl_ProjectID.SelectedValue;
        string[] projectId = ddl_ProjectID.SelectedItem.Text.Split('-');
        Orderno += projectId[0] + curYear + curMonth;
        string lastID = objData.GetSingleData("select Top 1(Right(requestid,4))as count from tbl_DCS where projectid='" + ddl_ProjectID.SelectedValue + "' order by id desc");
        int count = Convert.ToInt32(lastID);
        if (count > 0)
        {
            count++;
        }
        else
        {
            count = 0001;
        }
        Orderno += String.Format("{0:D4}", count);
        lbl_OrderNO_View.Text = Orderno;
        //int ToSlno = Int32.Parse(Orderno.ToString().Substring(16, 4));
        lbl_SlnoView.Value = String.Format("{0:D4}", count);

    }
    /// <summary>
    /// Save DCS
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void btn_Save_Click(object sender, EventArgs e)
    {
        string query, whoSubmited, dispatchDate = null;
        string receiveddate = null;
        whoSubmited = Session["Userid"].ToString();
        dispatchDate = Convert.ToString(DateTime.ParseExact(txt_DispatchDate.Text.Trim(), "dd/MM/yyyy", null).ToString("MM/dd/yyyy"));
        receiveddate = Convert.ToString(DateTime.ParseExact(txt_ReceivedDate.Text.Trim(), "dd/MM/yyyy", null).ToString("MM/dd/yyyy"));
        //Update
        string IfExists = objData.GetSingleData("select * from tbl_DCS where requestid='" + lbl_OrderNO_View.Text + "'");
        if (IfExists != "0")
        {
            query = String.Format("update [tbl_DCS] set department='{0}',projectid='{1}',Sno='{2}',dispatchdate='{3}',dispatchdescription='{4}',batchqty='{5}',dispatchqty='{6}',timetaken='{7}',inputdetails='{8}',outputdetails='{9}',Activity1='{10}',Activity2='{11}',Activity3='{12}',Activity4='{13}',Activity5='{14}',Activity6='{15}',CheckedBy='{16}',ApprovedBy='{17}',UpdatedDate=GETDATE(),UpdatedBy='{18}',projectname='{19}',receiveddate='{20}' where requestid='{21}'", txt_DepartmentName.Text, ddl_ProjectID.SelectedValue, lbl_SlnoView.Value, dispatchDate, txt_ScopeDispatchDesc.Text, txt_BatchQuantity.Text, txt_DispatchQuantity.Text, txt_TimeTaken.Text.Replace('_', '0'), txt_InputDetails.Text, txt_OutputDetails.Text, ddl_Activity1.SelectedValue, ddl_Activity2.SelectedValue, ddl_Activity3.SelectedValue, ddl_Activity4.SelectedValue, ddl_Activity5.SelectedValue, ddl_Activity6.SelectedValue, ddl_CheckedBy.SelectedItem.Text, txt_ApprovedBy.Text, whoSubmited, lbl_ProjectNameView.Text, receiveddate, lbl_OrderNO_View.Text);
            objData.DynamicParameters.Clear();
            objData.InsertOrUpdateData(query, false, false);
            ScriptManager.RegisterStartupScript(this, GetType(), "mismatch", "swal('DCS Updated Successfully','','success')", true);
            dcs_gridbind();
            Clear();
            ddl_ProjectID.Enabled = true;
        }
        //Insert
        else
        {
            query = "insert into tbl_DCS(requestid,department,projectid,projectname,Sno,receiveddate,dispatchdate,dispatchdescription,batchqty,dispatchqty,timetaken,inputdetails,outputdetails,Activity1,Activity2,Activity3,Activity4,Activity5,Activity6,Checkedby,Approvedby,createddate,status,SubmittedBy) values(@requestid,@department,@projectid,@projectname,@Sno,@receiveddate,@dispatchdate,@dispatchdescription,@batchqty,@dispatchqty,@timetaken,@inputdetails,@outputdetails,@Activity1,@Activity2,@Activity3,@Activity4,@Activity5,@Activity6,@CheckedBy,@Approvedby,GETDATE(),'1',@SubmittedBy)";
            objData.DynamicParameters.Clear();
            objData.DynamicParameters.Add("@requestid", lbl_OrderNO_View.Text);
            objData.DynamicParameters.Add("@department", txt_DepartmentName.Text);
            objData.DynamicParameters.Add("@projectid", ddl_ProjectID.SelectedValue);
            objData.DynamicParameters.Add("@projectname", lbl_ProjectNameView.Text);
            objData.DynamicParameters.Add("@Sno", lbl_SlnoView.Value);
            objData.DynamicParameters.Add("@receiveddate", receiveddate);
            objData.DynamicParameters.Add("@dispatchdate", dispatchDate);
            objData.DynamicParameters.Add("@dispatchdescription", txt_ScopeDispatchDesc.Text);
            objData.DynamicParameters.Add("@batchqty", txt_BatchQuantity.Text);
            objData.DynamicParameters.Add("@dispatchqty", txt_DispatchQuantity.Text);
            objData.DynamicParameters.Add("@timetaken", txt_TimeTaken.Text.ToString().Replace('_', '0'));
            objData.DynamicParameters.Add("@inputdetails", txt_InputDetails.Text);
            objData.DynamicParameters.Add("@outputdetails", txt_OutputDetails.Text);
            objData.DynamicParameters.Add("@Activity1", ddl_Activity1.SelectedValue);
            objData.DynamicParameters.Add("@Activity2", ddl_Activity2.SelectedValue);
            objData.DynamicParameters.Add("@Activity3", ddl_Activity3.SelectedValue);
            objData.DynamicParameters.Add("@Activity4", ddl_Activity4.SelectedValue);
            objData.DynamicParameters.Add("@Activity5", ddl_Activity5.SelectedValue);
            objData.DynamicParameters.Add("@Activity6", ddl_Activity6.SelectedValue);
            objData.DynamicParameters.Add("@CheckedBy", ddl_CheckedBy.SelectedItem.Text);
            objData.DynamicParameters.Add("@ApprovedBy", txt_ApprovedBy.Text);
            objData.DynamicParameters.Add("@SubmittedBy", whoSubmited);
            objData.InsertOrUpdateData(query, false, true);
            clsDataControl clsData = new clsDataControl();
            string projectId = clsData.GetSingleData("select IDENT_CURRENT('tbl_dcs')");
            udSlnoGeneration(projectId);
            ScriptManager.RegisterStartupScript(this, GetType(), "mismatch", "swal('DCS Inserted Successfully','','success')", true);
            Clear();
            dcs_gridbind();
        }
    }

    /// <summary>
    /// DCS Generation
    /// </summary>
    /// <param name="ss"></param>

    void udSlnoGeneration(string ss)
    {
        SqlTransaction SqlTrans = null;
        int DCNo = 0;
        string orderNo = "";
        string[] projectId = ddl_ProjectID.SelectedItem.Text.Split('-');
        string pId = Convert.ToString(projectId[0]);
        if (lbl_OrderNO_View.Visible == true) orderNo = lbl_OrderNO_View.Text;
        using (SqlConnection dcscon = new SqlConnection(ConfigurationManager.ConnectionStrings["DCSconn"].ConnectionString))
        {
            if (dcscon.State == ConnectionState.Closed) dcscon.Open();

            string excelname = ddl_ProjectID.SelectedValue.ToString() + " - Dispatch Control Sheet_" + Convert.ToString(Convert.ToDateTime(txt_DispatchDate.Text.Trim()).ToString("MM-dd-yyyy")) + ".xls";

            SqlTrans = dcscon.BeginTransaction();
            SqlCommand cmd = new SqlCommand("sp_dcsonline", dcscon, SqlTrans);
            cmd.Parameters.Clear();
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.AddWithValue("@SelectedMonth", Convert.ToString(Convert.ToDateTime(txt_DispatchDate.Text.Trim()).ToString("MM")));
            cmd.Parameters.AddWithValue("@CurDate", Convert.ToString(DateTime.Now.ToString("MM/dd/yyyy hh:mm tt")));
            cmd.Parameters.AddWithValue("@DcsDate", Convert.ToString(Convert.ToDateTime(txt_DispatchDate.Text.Trim()).ToString("MM/dd/yyyy")));
            cmd.Parameters.AddWithValue("@ProjectID", Convert.ToString(projectId[0]));
            cmd.Parameters.AddWithValue("@Department", "WEB");
            cmd.Parameters.AddWithValue("@LOLorLDS", Convert.ToString(char.IsNumber(Convert.ToChar(pId.Trim().Substring(0, 1))) ? "LDS" : "LOL"));
            cmd.Parameters.AddWithValue("@IP", Convert.ToString(System.Environment.UserDomainName) + "^" + Convert.ToString(System.Environment.UserName) + "^" + Convert.ToString(System.Net.Dns.GetHostName()) + "^" + Convert.ToString(System.Net.Dns.GetHostEntry(System.Net.Dns.GetHostName()).AddressList[0]));
            cmd.Parameters.AddWithValue("@Filename", Convert.ToString(excelname));

            SqlParameter DcsNO = new SqlParameter();
            DcsNO.Direction = ParameterDirection.Output;
            DcsNO.SqlDbType = SqlDbType.BigInt;
            DcsNO.ParameterName = "DcsNO";
            cmd.Parameters.Add(@DcsNO);
            cmd.ExecuteNonQuery();
            DCNo = Convert.ToInt32(DcsNO.Value);
            SqlTrans.Commit();
        }
        string DCSAutono = DCNo.ToString("0000") + " - " + Convert.ToString(Convert.ToDateTime(txt_DispatchDate.Text.Trim()).ToString("MM")) + " - " + Convert.ToString(Convert.ToDateTime(txt_DispatchDate.Text.Trim()).ToString("yy")) + " - E - " + Convert.ToString(char.IsNumber(Convert.ToChar(ddl_ProjectID.SelectedItem.Text.Substring(0, 1))) ? "LDS" : "LDSC");
        /*using (SqlConnection con1 = new SqlConnection(ConfigurationManager.ConnectionStrings["conn"].ConnectionString))
        {
            con1.Open();
            string inser = "update tbl_dcs set Sno ='" + DCSAutono + "' where id =" + ss + " and ProjectId='" + ddl_ProjectID.SelectedValue + "' and requestid='" + orderNo + "'";
            SqlCommand cmd_inser1 = new SqlCommand(inser, con1);
            cmd_inser1.ExecuteNonQuery();
        }*/
        clsDataControl clsdata = new clsDataControl();
        clsdata.InsertOrUpdateData("update tbl_dcs set Sno ='" + DCSAutono + "' where id =" + ss + " and ProjectId='" + ddl_ProjectID.SelectedValue + "' and requestid='" + orderNo + "'", false, false);

    }

    /// <summary>
    /// clear Form Elements after submit data
    /// </summary>
    protected void Clear()
    {
        lbl_OrderNO_View.Text = "NA";
        lbl_ProjectNameView.Text = "NA";
        lbl_SlnoView.Value = "";
        ddl_ProjectID.SelectedIndex = 0;
        txt_DepartmentName.Text = string.Empty;
        txt_DispatchDate.Text = string.Empty;
        txt_ScopeDispatchDesc.Text = string.Empty;
        txt_BatchQuantity.Text = string.Empty;
        txt_DispatchQuantity.Text = string.Empty;
        txt_TimeTaken.Text = string.Empty;
        txt_InputDetails.Text = string.Empty;
        txt_OutputDetails.Text = string.Empty;
        ddl_Activity1.SelectedIndex = 0;
        ddl_Activity2.SelectedIndex = 0;
        ddl_Activity3.SelectedIndex = 0;
        ddl_Activity4.SelectedIndex = 0;
        ddl_Activity5.SelectedIndex = 0;
        ddl_Activity6.SelectedIndex = 0;
        ddl_CheckedBy.SelectedIndex = 0;
        btn_Save.Text = "Save";
    }
    /// <summary>
    ///  Cancel Button Click,Clear form elements
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void btn_Cancel_Click(object sender, EventArgs e)
    {
        ddl_ProjectID.Enabled = true;
        Clear();
    }
    /// <summary>
    /// Currently Not in Use
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void lnk_TaskStatus_Click(object sender, EventArgs e)
    {

    }
    /// <summary>
    /// Currently Not in Use
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void SelectCurrentData(object sender, EventArgs e)
    {

    }
    /// <summary>
    /// Currently Not in Use
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void ddl_TaskStateChange_SelectedIndexChanged(object sender, EventArgs e)
    {

    }
    /// <summary>
    /// DCS Delete from gridview
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void grdDCS_RowDeleting(object sender, GridViewDeleteEventArgs e)
    {
        try
        {
            string confirmValue = Request.Form["confirm_value"];
            if (confirmValue == "Yes")
            {
                String id = (string)grd_DCS.DataKeys[e.RowIndex].Values[0].ToString();
                con.Open();
                com_Check.Connection = con;
                com_Check.CommandText = "update tbl_DCS set status=0 where id ='" + id + "'";
                com_Check.ExecuteNonQuery();
                dcs_gridbind();
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
    /// Edit Record from Saved Record from Database
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void grd_DCS_SelectedIndexChanged(object sender, EventArgs e)
    {
        try
        {
            ddl_ProjectID.Enabled = false;
            GridViewRow row = grd_DCS.SelectedRow;
            lbl_OrderNO_View.Text = ((Label)row.FindControl("lbl_orderno")).Text;
            txt_DepartmentName.Text = ((Label)row.FindControl("lbl_Department")).Text;
            ddl_ProjectID.SelectedValue = ((Label)row.FindControl("lbl_Projectid")).Text;
            lbl_ProjectNameView.Text = ((Label)row.FindControl("lbl_projectname")).Text;
            lbl_SlnoView.Value = ((Label)row.FindControl("lbl_slno")).Text;
            txt_ReceivedDate.Text = ((Label)row.FindControl("lbl_ReceivedDate")).Text;
            txt_DispatchDate.Text = ((Label)row.FindControl("lbl_DispatchDate")).Text;
            txt_ScopeDispatchDesc.Text = ((HiddenField)row.FindControl("hdn_DispatchDescription")).Value;
            txt_BatchQuantity.Text = ((Label)row.FindControl("lbl_BatchQty")).Text;
            txt_DispatchQuantity.Text = ((Label)row.FindControl("lbl_DispatchQty")).Text;
            txt_TimeTaken.Text = ((Label)row.FindControl("lbl_TimeTaken")).Text;
            txt_InputDetails.Text = ((HiddenField)row.FindControl("hdn_InputDetails")).Value;
            txt_OutputDetails.Text = ((HiddenField)row.FindControl("hdn_OutputDetails")).Value;
            bindDropDowns();
            ddl_Activity1.SelectedValue = ((HiddenField)row.FindControl("hdn_Activity1")).Value;
            ddl_Activity2.SelectedValue = ((HiddenField)row.FindControl("hdn_Activity2")).Value;
            ddl_Activity3.SelectedValue = ((HiddenField)row.FindControl("hdn_Activity3")).Value;
            ddl_Activity4.SelectedValue = ((HiddenField)row.FindControl("hdn_Activity4")).Value;
            ddl_Activity5.SelectedValue = ((HiddenField)row.FindControl("hdn_Activity5")).Value;
            ddl_Activity6.SelectedValue = ((HiddenField)row.FindControl("hdn_Activity6")).Value;
            ddl_CheckedBy.SelectedItem.Text = ((HiddenField)row.FindControl("hdn_CheckedBy")).Value;
            txt_ApprovedBy.Text = ((HiddenField)row.FindControl("hdn_ApprovedBy")).Value;
            btn_Save.Text = "Update";
        }
        catch (Exception ex)
        {
        }
    }
    /// <summary>
    /// Gridview Bind DCS
    /// </summary>
    protected void dcs_gridbind()
    {
        try
        {
            con.Close();
            SqlDataAdapter da = new SqlDataAdapter();
            string Rights = Convert.ToString(Session["Rights"]);
            if (Rights == "Team Leader")
            {
                con2.Close();
                con2.Open();
                com_Check.Connection = con2;
                com_Check.CommandText = "select TeamID from tbl_teamAllotmentMaster where teamleader like  ('%" + Session["Userid"] + "%')";
                SqlDataReader dr = com_Check.ExecuteReader();
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
                com_Check.Connection = con;
                com_DCS.CommandText = "select a.*,e.manualid from tbl_DCS a inner join tbl_ProjectReq e on e.manualid=a.projectid and (";
                foreach (string Splitted in SplittedProjectAlloted)
                {
                    com_DCS.CommandText += "e.allotedteamid like '%" + Splitted + "%' or ";
                }
                com_DCS.CommandText = com_DCS.CommandText.Substring(0, com_DCS.CommandText.Length - 3);
                com_DCS.CommandText += ") and e.projectid!='NA'";
                if (ddl_ProjectID.SelectedValue != "NA")
                {
                    com_DCS.CommandText += " and e.manualid='" + ddl_ProjectID.SelectedValue + "'";
                }
                com_DCS.CommandText += "and a.status=1 order by a.id desc";
                da = new SqlDataAdapter(com_DCS.CommandText, con);

                DataSet ds = new DataSet();
                da.Fill(ds);
                if (ds.Tables[0].Rows.Count > 0)
                {
                    grd_DCS.DataSource = ds;
                    grd_DCS.DataBind();
                }
                else
                {
                    ds.Tables[0].Rows.Add(ds.Tables[0].NewRow());
                    grd_DCS.DataSource = ds;
                    grd_DCS.DataBind();
                    int columncount = grd_DCS.Rows[0].Cells.Count;
                    grd_DCS.Rows[0].Cells.Clear();
                    grd_DCS.Rows[0].Cells.Add(new TableCell());
                    grd_DCS.Rows[0].Cells[0].ColumnSpan = columncount;
                    grd_DCS.Rows[0].Cells[0].Text = "<center>----- No Records Found -----</center>";
                }
            }
            else
            {
                com_DCS.CommandText = "select a.*,e.manualid from tbl_DCS a  inner join tbl_ProjectReq e on e.manualid=a.projectid where a.status=1";
                if (ddl_ProjectID.SelectedValue != "NA")
                {
                    com_DCS.CommandText += " and a.projectid='" + ddl_ProjectID.SelectedValue + "'";
                }
                com_DCS.CommandText += "order by a.id desc";
                da = new SqlDataAdapter(com_DCS.CommandText, con);
                DataSet ds = new DataSet();
                da.Fill(ds);
                if (ds.Tables[0].Rows.Count > 0)
                {
                    grd_DCS.DataSource = ds;
                    grd_DCS.DataBind();
                }

                else
                {
                    ds.Tables[0].Rows.Add(ds.Tables[0].NewRow());
                    grd_DCS.DataSource = ds;
                    grd_DCS.DataBind();
                    int columncount = grd_DCS.Rows[0].Cells.Count;
                    grd_DCS.Rows[0].Cells.Clear();
                    grd_DCS.Rows[0].Cells.Add(new TableCell());
                    grd_DCS.Rows[0].Cells[0].ColumnSpan = columncount;
                    grd_DCS.Rows[0].Cells[0].Text = "<center>----- No Records Found -----</center>";
                }
            }
        }
        catch (Exception ex)
        {
        }
    }
    /// <summary>
    /// Download DCS Report
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void lnk_Download_Click(object sender, EventArgs e)
    {
        try
        {
            clsDataControl clsdata = new clsDataControl();
            LinkButton btn = (LinkButton)(sender);
            string selectedDCS = btn.CommandArgument;
            con.Close();
            con.Open();
            com_DCS.Connection = con;
            com_DCS.CommandText = "select *,convert(varchar(10),isnull(receiveddate,(select receiveddate from tbl_ProjectReq where manualid='" + ddl_ProjectID.SelectedValue + "')),103)[receiveddate2],convert(Varchar(10),dispatchdate,103)[Dispatch Date],(select username from tbl_usermaster b where b.userid=activity1)[Activity1by],(select username from tbl_usermaster b where b.userid=activity2)[Activity2by],(select username from tbl_usermaster b where b.userid=activity3)[Activity3by],(select username from tbl_usermaster b where b.userid=activity4)[Activity4by],(select username from tbl_usermaster b where b.userid=activity5)[Activity5by],(select username from tbl_usermaster b where b.userid=activity6)[Activity6by] from tbl_DCS where requestid='" + selectedDCS + "'";
            SqlDataReader dr = com_DCS.ExecuteReader();
            if (dr.HasRows)
            {
                dr.Read();
                Response.ClearContent();
                string excelname = "DCS_Details.xls";
                string teamVerify = "";
                int toSpan = 1;
                string manualId = clsdata.GetSingleData("select manualid from tbl_projectReq where manualid ='" + dr["projectid"].ToString() + "'");
                string filePath = HttpContext.Current.Server.MapPath(".") + "\\DCS_Reports\\";
                if (!Directory.Exists(filePath))
                    Directory.CreateDirectory(filePath + "\\DCS_Reports\\");
                string excelpath = filePath + ("DCS_Report" + DateTime.Now.ToShortDateString().Replace("/", "") + ".xls");
                if (File.Exists(excelpath))
                    File.Delete(excelpath);
                FileInfo file = new FileInfo(excelpath);
                string reportName = "DCS Report" + DateTime.Now.ToString("dd/MM/yyyy");
                StringBuilder sb = new StringBuilder();
                sb.Append("<html>");
                sb.Append("<head>");
                sb.Append("<body>");
                sb.Append("<table border='1' style='font-family:Calibri; font-size:14.7px;'>");
                sb.Append("<tr><td align='center' rowspan='2' style='text-align:center'><img src = '" + Server.MapPath("~") + "\\assets\\img\\logo.png'  alt='Logo' border=3 style='float:right;margin-left:50px' height=45></td><td colspan='3' style='text-align:center;font-weight:bold;'>" + "Own" + "</td><td colspan='3' style='text-align:center;font-weight:bold;'>" + "OP08" + "</td></tr>");
                sb.Append("<tr><td colspan='3' style='text-align:center;font-weight:bold;'>" + "DISPATCH CONTROL SHEET" + "</td><td colspan='3' style='text-align:center;font-weight:bold;'>" + "DC" + "</td></tr>");
                sb.Append("<tr><td style='font-weight:bold'>1. Order No</td><td style='text-align:left;'>" + dr["requestid"].ToString().ToUpper() + "</td>");
                sb.Append("<td style='text-align:left;font-weight:bold' colspan='2'>2. Department Name</td><td colspan='3' style='text-align:left;'>" + dr["department"].ToString().ToUpper() + "</td></tr>");
                sb.Append("<tr><td style='font-weight:bold'>3. Project ID</td><td style='text-align:left;'>" + manualId.ToUpper() + "</td>");
                sb.Append("<td style='font-weight:bold' colspan='2'>4. Project Name</td><td colspan='3' style='text-align:left;'>" + dr["projectname"].ToString().ToUpper() + "</td></tr>");
                sb.Append("<tr><td style='font-weight:bold'>5. Sl.No.</td><td style='text-align:left;'>" + dr["Sno"].ToString() + "</td>");
                sb.Append("<td style='font-weight:bold' colspan='2'>6. Received Date</td><td style='text-align:left;'>" + dr["receiveddate2"].ToString() + "</td><td style='font-weight:bold'>7. Dispatch Date</td><td style='text-align:left;'>" + dr["Dispatch Date"].ToString() + "</td></tr>");
                sb.Append("<tr><td style='font-weight:bold'>8. Scope / Dispatch Description</td><td style='text-align:left;' colspan='5'>" + dr["dispatchdescription"].ToString() + "</td></tr>");
                sb.Append("<tr><td style='font-weight:bold'>9. Batch Qty.</td><td style='text-align:left;'>" + dr["batchqty"].ToString() + "</td>");
                sb.Append("<td style='font-weight:bold' colspan='2'>10. Dispatch Qty.</td><td style='text-align:left;'>" + dr["dispatchqty"].ToString().ToUpper() + "</td>");
                sb.Append("<td style='font-weight:bold'>11. Time Taken</td><td style='text-align:left;'>" + dr["timetaken"].ToString().ToUpper() + " Hours</td></tr>");
                sb.Append("<tr><td style='font-weight:bold'>12. Input Details</td><td style='text-align:left;'>" + dr["inputdetails"].ToString() + "</td>");
                sb.Append("<td style='font-weight:bold' colspan='2'>13. Output Details</td><td style='text-align:left;' colspan='3'>" + dr["outputdetails"].ToString() + "</td></tr>");
                sb.Append("<tr><td colspan='7'></td></tr>");
                sb.Append("<tr><td style='text-align:center;font-weight:bold' colspan='2'>Activity</td><td colspan='5' style='text-align:center;font-weight:bold'>Signature</td></tr>");
                sb.Append("<tr><td style='font-weight:bold' colspan='2'>Dispatch Qty Measured by</td><td colspan='5' style='text-align:left;'>" + dr["Activity1by"].ToString() + "</td></tr>");
                sb.Append("<tr><td style='font-weight:bold' colspan='2'>Discrepancy Report Created by</td><td colspan='5' style='text-align:;left;'>" + dr["Activity2by"].ToString() + "</td></tr>");
                sb.Append("<tr><td style='font-weight:bold' colspan='2'>Discrepancy Report Checked by</td><td colspan='5' style='text-align:left;'>" + dr["Activity3by"].ToString() + "</td></tr>");
                sb.Append("<tr><td style='font-weight:bold' colspan='2'>Packed by</td><td colspan='5' style='text-align:left;'>" + dr["Activity4by"].ToString() + "</td></tr>");
                sb.Append("<tr><td style='font-weight:bold' colspan='2'>Uploaded by</td><td colspan='5' style='text-align:left;'>" + dr["Activity5by"].ToString() + "</td></tr>");
                sb.Append("<tr><td style='font-weight:bold' colspan='2'>Discrepancy and Message Sent by</td><td colspan='5' style='text-align:left;'>" + dr["Activity6by"].ToString() + "</td></tr>");
                sb.Append("<tr><td  style='text-align:center;font-weight:bold' colspan='7'>Details mentioned above has been checked and approved for invoicing.</td></tr>");
                sb.Append("<tr><td style='text-align:center;font-weight:bold'>Checked by</td><td style='text-align:center;'>" + dr["checkedby"].ToString() + "</td>");
                sb.Append("<td style='font-weight:bold'>Approved by</td><td colspan='4' style='text-align:center;'>" + dr["approvedby"].ToString() + "</td></tr>");
                sb.Append("</table>");
                sb.Append("</body>");
                sb.Append("</head>");
                sb.Append("</html>");
                File.WriteAllText(excelpath, sb.ToString());
                Response.Clear();
                Response.AddHeader("Content-Length", file.Length.ToString(CultureInfo.InvariantCulture));
                Response.AddHeader("Content-Disposition", "attachment;filename=\"" + ("DCS_Report.xls") + "\"");
                Response.ContentType = "application/octet-stream";
                Response.Flush();
                Response.TransmitFile(filePath + ("DCS_Report" + DateTime.Now.ToShortDateString().Replace("/", "") + ".xls"));
                Response.End();
            }
        }
        catch (Exception ex)
        {

        }
    }
    /// <summary>
    /// To Avoid Dispatch Date to Not Exceed Received Date
    /// </summary>
    /// <returns>The received date text changed.</returns>
    /// <param name="sender">Sender.</param>
    /// <param name="e">E.</param>
    protected void txt_ReceivedDate_TextChanged(object sender, EventArgs e)
    {
        calext_DispatchDate.StartDate = Convert.ToDateTime(txt_ReceivedDate.Text.ToString());
        dcs_gridbind();
    }
}