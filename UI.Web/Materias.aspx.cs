﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Business.Entities;
using Business.Logic;

namespace UI.Web
{
    public partial class Materias : System.Web.UI.Page
    {
        private MateriaLogic _logic;

        public MateriaLogic Logic
        {
            get
            {
                if (_logic == null)
                {
                    _logic = new MateriaLogic();
                }
                return _logic;
            }
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!Page.IsPostBack)
            {
                LoadGrid();
            }
            if (this.gridView.Rows.Count > 0)
            {
                this.gridView.HeaderRow.TableSection = TableRowSection.TableHeader;
            }
        }

        private void LoadGrid()
        {
            PlanLogic plan = new PlanLogic();

            this.gridView.DataSource = this.Logic.GetAll();
            this.gridView.DataBind();

            if (this.planDropDown.Items.Count == 1)
            {
                this.planDropDown.DataSource = plan.GetAll();
                this.planDropDown.DataTextField = "Descripcion";
                this.planDropDown.DataValueField = "ID";
                this.planDropDown.DataBind();
            }           
        }

        public enum FormModes
        {
            alta,
            baja,
            modificacion
        }

        public FormModes FormMode
        {
            get { return (FormModes)this.ViewState["FormMode"]; }
            set { this.ViewState["FormMode"] = value; }
        }

        private Materia Entity { get; set; }

        private int SelectedID
        {
            get
            {
                if (this.ViewState["SelectedID"] != null)
                {
                    return (int)this.ViewState["SelectedID"];
                }
                else
                {
                    return 0;
                }
            }
            set
            {
                this.ViewState["SelectedID"] = value;
            }
        }

        private bool IsEntitySelected
        {
            get
            {
                return (this.SelectedID != 0);
            }
        }

        protected void gridView_SelectedIndexChanged(object sender, EventArgs e)
        {
            this.SelectedID = (int)this.gridView.SelectedValue;

        }
        private void LoadForm(int ID)
        {
            this.Entity = this.Logic.GetOne(ID);
            this.descripcionTextBox.Text = this.Entity.Descripcion;
            this.hssemTextBox.Text = this.Entity.HSSemanales.ToString();
            this.hstotTextBox.Text = this.Entity.HSTotales.ToString();
            this.planDropDown.SelectedValue = this.Entity.Plan.ID.ToString();
        }

        protected void editarlinkButton_Click(object sender, EventArgs e)
        {
            if (this.IsEntitySelected)
            {
                this.EnableForm(true);
                this.formPanel.Visible = true;
                this.formActionPanel.Visible = true;
                this.FormMode = FormModes.modificacion;
                this.LoadForm(this.SelectedID);
            }
        }

        private void LoadEntity(Materia materia)
        {
            materia.Descripcion = this.descripcionTextBox.Text;
            materia.HSSemanales = int.Parse(this.hssemTextBox.Text);
            materia.HSTotales = int.Parse(this.hstotTextBox.Text);

            materia.Plan = new Plan();
            materia.Plan.ID = int.Parse(this.planDropDown.SelectedItem.Value);
        }

        private void SaveEntity(Materia materia)
        {
            this.Logic.Save(materia);
        }

        protected void aceptarLinkButton_Click(object sender, EventArgs e)
        {
            switch (this.FormMode)
            {
                case FormModes.baja:
                    this.DeleteEntity(this.SelectedID);
                    this.LoadGrid();
                    break;
                case FormModes.modificacion:
                    this.Entity = new Materia();
                    this.Entity.ID = this.SelectedID;
                    this.Entity.State = BusinessEntity.States.Modified;
                    this.LoadEntity(this.Entity);
                    this.SaveEntity(this.Entity);
                    this.LoadGrid();
                    break;
                case FormModes.alta:
                    this.Entity = new Materia();
                    this.LoadEntity(this.Entity);
                    this.SaveEntity(this.Entity);
                    this.LoadGrid();
                    break;
                default:
                    break;
            }

            this.formPanel.Visible = false;
            this.formActionPanel.Visible = false;

            //probando limpiar seleccion
            this.gridView.SelectedIndex = -1;
            this.SelectedID = 0;
        }

        private void EnableForm(bool enable)
        {
            this.descripcionTextBox.Enabled = enable;
            this.hssemTextBox.Enabled = enable;
            this.hstotTextBox.Enabled = enable;

            this.planDropDown.Enabled = enable;
        }

        protected void eliminarLinkButton_Click(object sender, EventArgs e)
        {
            if (this.IsEntitySelected)
            {
                this.formPanel.Visible = true;
                this.formActionPanel.Visible = true;
                this.FormMode = FormModes.baja;
                this.EnableForm(false);
                this.LoadForm(this.SelectedID);
            }
        }

        private void DeleteEntity(int ID)
        {
            this.Logic.Delete(ID);
        }
        protected void nuevoLinkButton_Click(object sender, EventArgs e)
        {
            this.formPanel.Visible = true;
            this.formActionPanel.Visible = true;
            this.FormMode = FormModes.alta;
            this.ClearForm();
            this.EnableForm(true);
        }
        private void ClearForm()
        {
            this.descripcionTextBox.Text = string.Empty;
            this.hssemTextBox.Text = string.Empty;
            this.hstotTextBox.Text = string.Empty;
            
            this.planDropDown.SelectedIndex = 0;

        }
        protected void cancelarLinkButtom_Click(object sender, EventArgs e)
        {
            this.formActionPanel.Visible = false;
            this.formPanel.Visible = false;

            //Probando limpiar seleccion
            this.gridView.SelectedIndex = -1;
            this.SelectedID = 0;
        }
    }
}