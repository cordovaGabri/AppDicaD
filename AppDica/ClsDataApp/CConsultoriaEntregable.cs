﻿using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using ClsInterface;

namespace ClsDataApp
{
    public class CConsultoriaEntregable : CSqlvars
    {
        public CConsultoriaEntregable(string ConexionData)
        {
            _ConexionData = ConexionData;
        }

        public ClsDataSets.DS_TB_EMP Detalle(int Id, int IdConsultoria, string NombEntregable, string Descripcion, DateTime FechEntrega,
            string Duracion, char EstadoEntregable, string UrlEntregable, string ObservacionEntregable, string IdAspirante, string UsuaCrea, 
            DateTime FechCrea, string UsuaActu, DateTime FechActu, int OpcionConsulta)
        {
            ClsDataSets.DS_TB_EMP objDataSet = new ClsDataSets.DS_TB_EMP();
            try
            {
                ObjConnection = new SqlConnection(_ConexionData);
                ObjAdapter = new SqlDataAdapter("SP_TB_CONSULTORIA_ENTREGABLE_GetByAll", ObjConnection);
                ObjParam = new SqlParameter();
                ObjAdapter.SelectCommand.CommandType = CommandType.StoredProcedure;

                ObjAdapter.SelectCommand.Parameters.AddWithValue("@ID", Id);
                ObjAdapter.SelectCommand.Parameters.AddWithValue("@ID_CONSULTORIA", IdConsultoria);
                ObjAdapter.SelectCommand.Parameters.AddWithValue("@DS_ENTREGABLE", NombEntregable);
                ObjAdapter.SelectCommand.Parameters.AddWithValue("@DS_DESCRIPCION_ENT", Descripcion);
                ObjAdapter.SelectCommand.Parameters.AddWithValue("@FECH_ENTREGA_ENT", FechEntrega);
                ObjAdapter.SelectCommand.Parameters.AddWithValue("@DS_DURACION_ENT", Duracion);
                ObjAdapter.SelectCommand.Parameters.AddWithValue("@CD_ESTADO_ENTREGABLE", EstadoEntregable);
                ObjAdapter.SelectCommand.Parameters.AddWithValue("@URL_ENTREGABLE", UrlEntregable);
                ObjAdapter.SelectCommand.Parameters.AddWithValue("@DS_OBSERVACION_ENTREGABLE", ObservacionEntregable);
                ObjAdapter.SelectCommand.Parameters.AddWithValue("@ID_ASPIRANTE", IdAspirante);
                ObjAdapter.SelectCommand.Parameters.AddWithValue("@USUA_CREA", UsuaCrea);
                ObjAdapter.SelectCommand.Parameters.AddWithValue("@FECH_CREA", FechCrea);
                ObjAdapter.SelectCommand.Parameters.AddWithValue("@USUA_ACTU", UsuaActu);
                ObjAdapter.SelectCommand.Parameters.AddWithValue("@FECH_ACTU", FechActu);
                ObjAdapter.SelectCommand.Parameters.AddWithValue("@OPCI_CONS", OpcionConsulta);

                ObjAdapter.Fill(objDataSet, "TB_CONSULTORIA_ENTREGABLE");

                ObjConnection.Close();
                if (ObjConnection.State != ConnectionState.Closed)
                {
                    ObjConnection.Close();
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }

            return objDataSet;
        }
        public DataQuery Actualizacion(int Id, int IdConsultoria, string NombEntregable, string Descripcion, DateTime FechEntrega,
            string Duracion, char EstadoEntregable, string UrlEntregable, string ObservacionEntregable, string IdAspirante, 
            string LoginUsuario, TipoActualizacion OpcionActualizacion)
        {
            DataQuery objResultado = new DataQuery();
            try
            {
                string StrCommand = "";

                switch (OpcionActualizacion)
                {
                    case TipoActualizacion.Adicionar:
                        StrCommand = "SP_TB_CONSULTORIA_ENTREGABLE_INSERT";
                        break;
                    case TipoActualizacion.Actualizar:
                        StrCommand = "SP_TB_CONSULTORIA_ENTREGABLE_UPDATE";
                        break;
                    case TipoActualizacion.Eliminar:
                        StrCommand = "";
                        break;
                    case TipoActualizacion.No_Definida:
                        objResultado.CodigoError = -1;
                        objResultado.MensajeError = "Opcion de Actualizacion No Definida. Objeto COpcionesSistemas. Metodo Actualizacion";
                        //return objResultado;
                        break;
                }

                ObjConnection = new SqlConnection(_ConexionData);

                ObjCommand = new SqlCommand(StrCommand, ObjConnection);
                ObjParam = new SqlParameter();
                ObjCommand.CommandType = CommandType.StoredProcedure;

                if (OpcionActualizacion == TipoActualizacion.Adicionar)
                {
                    ObjParam = ObjCommand.Parameters.Add("@ID", SqlDbType.Int, 0);
                    ObjParam.Direction = ParameterDirection.Output;
                }
                else
                {
                    ObjCommand.Parameters.AddWithValue("@ID", Id);
                }
                ObjCommand.Parameters.AddWithValue("@ID_CONSULTORIA", IdConsultoria);
                ObjCommand.Parameters.AddWithValue("@DS_ENTREGABLE", NombEntregable);
                ObjCommand.Parameters.AddWithValue("@DS_DESCRIPCION_ENT", Descripcion);
                ObjCommand.Parameters.AddWithValue("@FECH_ENTREGA_ENT", FechEntrega);
                ObjCommand.Parameters.AddWithValue("@DS_DURACION_ENT", Duracion);
                ObjCommand.Parameters.AddWithValue("@CD_ESTADO_ENTREGABLE", EstadoEntregable);
                ObjCommand.Parameters.AddWithValue("@URL_ENTREGABLE", UrlEntregable);
                ObjCommand.Parameters.AddWithValue("@DS_OBSERVACION_ENTREGABLE", ObservacionEntregable);
                ObjCommand.Parameters.AddWithValue("@ID_ASPIRANTE", IdAspirante);
                ObjCommand.Parameters.AddWithValue("@LOGIN_USUARIO", LoginUsuario);

                ObjParam = ObjCommand.Parameters.Add("@FILAS_AFECTADAS", SqlDbType.Int, 0);
                ObjParam.Direction = ParameterDirection.Output;

                ObjParam = ObjCommand.Parameters.Add("@NumeroError", SqlDbType.Decimal);
                ObjParam.Precision = 38;
                ObjParam.Scale = 0;
                ObjParam.Direction = ParameterDirection.Output;

                ObjParam = ObjCommand.Parameters.Add("@MensajeError", SqlDbType.NVarChar, 4000);
                ObjParam.Direction = ParameterDirection.Output;

                ObjConnection.Open();
                ObjCommand.ExecuteNonQuery();

                objResultado.CodigoAuxiliar = (object)ObjCommand.Parameters["@ID"].Value;
                objResultado.FilasAfectadas = (int)ObjCommand.Parameters["@FILAS_AFECTADAS"].Value;
                objResultado.CodigoError = (decimal)ObjCommand.Parameters["@NumeroError"].Value;
                objResultado.MensajeError = (string)ObjCommand.Parameters["@MensajeError"].Value;

                ObjConnection.Close();

                if (ObjConnection.State != ConnectionState.Closed)
                {
                    ObjConnection.Close();
                }
            }
            catch (Exception ex)
            {
                objResultado.CodigoError = -1;
                objResultado.MensajeError = ex.Message;
            }

            return objResultado;
        }
    }
}