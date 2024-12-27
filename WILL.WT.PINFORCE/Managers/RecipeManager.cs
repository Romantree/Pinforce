using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using WILL.WT.PINFORCE.Models;
using WILL.WT.PINFORCE.Models.Recipe;
using TS.FW;
using TS.FW.Wpf.v2.Controls.InPut;
using TS.FW.Wpf.v2.Core;
using System.Diagnostics;

namespace WILL.WT.PINFORCE.Managers
{
    public enum RecipeType
    {
        MAIN,
        AUTO_CONTACT,
    }

    public class RecipeManager
    {
        private const string ROOT = @"..\Recipe";

        public AutoContactRcpModel AutoContact { get; private set; }

        public Response NewRecipe(RecipeType type, string name)
        {
            try
            {
                var no = this.ToRecipeNo(type);
                IRecipeModel model = null;

                switch (type)
                {
                    case RecipeType.MAIN:
                        {
                            model = new MainRecipeModel()
                            {
                                No = no,
                                Name = name,
                            };
                        }
                        break;
                    case RecipeType.AUTO_CONTACT:
                        {
                            model = new AutoContactRcpModel()
                            {
                                No = no,
                                Name = name,
                                RepeatCount = 1,
                            };
                        }
                        break;
                }

                if (model == null) return new Response(false, $"등록되지 않은 레시피 유형 입니다. {type}");

                return this.Save(model);
            }
            catch (Exception ex)
            {
                Logger.Write(this, ex);
                return ex;
            }
        }

        public void InitPath()
        {
            if (Directory.Exists(ROOT) == false) Directory.CreateDirectory(ROOT);

            foreach (RecipeType item in Enum.GetValues(typeof(RecipeType)))
            {
                var path = Path.Combine(ROOT, item.ToString());
                if (Directory.Exists(path)) continue;

                Directory.CreateDirectory(path);
            }
        }

        public void LoadDatabase()
        {
            try
            {
                var rcp = this.GetRecipe(RecipeType.AUTO_CONTACT, 0) as AutoContactRcpModel;
                if (rcp == null)
                {
                    var res = NewRecipe(RecipeType.AUTO_CONTACT, "AutoContact");
                    if (res == false)
                    {
                        Logger.Write(this, res.Comment, Logger.LogEventLevel.Error);
                    }
                    else
                    {
                        AutoContact = this.GetRecipe(RecipeType.AUTO_CONTACT, 0) as AutoContactRcpModel;
                    }
                }
                else
                {
                    AutoContact = rcp;
                }

            }
            catch (Exception ex)
            {
                Logger.Write(this, ex);
            }
        }

        public List<IRecipeModel> ToRecipeList(RecipeType type)
        {
            var list = new List<IRecipeModel>();

            try
            {
                foreach (var file in this.ToFileList(type))
                {
                    if (int.TryParse(this.ToFileName(file), out int no) == false) continue;

                    var item = this.GetRecipe(type, no);
                    if (item == null) continue;

                    list.Add(item);
                }
            }
            catch (Exception ex)
            {
                Logger.Write(this, ex);
            }

            return list;
        }

        public T Reload<T>(IRecipeModel model) where T : IRecipeModel
        {
            if (model == null) return null;

            var rcp = this.GetRecipe(model.Type, model.No);
            if (rcp == null) return null;

            return (T)rcp;
        }

        public Response Save(IRecipeModel model)
        {
            try
            {
                var file = this.ToFile(model.Type, model.No);
                if (File.Exists(file)) File.Delete(file);

                return TS.FW.Serialization.Serialization.JsonSerializerFile(model, file);
            }
            catch (Exception ex)
            {
                Logger.Write(this, ex);
                return ex;
            }
        }

        public Response SaveAs(string name, IRecipeModel model)
        {
            try
            {
                var no = this.ToRecipeNo(model.Type);
                model.No = no;
                model.Name = name;

                return this.Save(model);
            }
            catch (Exception ex)
            {
                Logger.Write(this, ex);
                return ex;
            }
        }

        public Response Delete(IRecipeModel model)
        {
            try
            {
                var file = this.ToFile(model.Type, model.No);
                if (File.Exists(file)) File.Delete(file);

                return new Response();
            }
            catch (Exception ex)
            {
                Logger.Write(this, ex);
                return ex;
            }
        }

        private void Clear(RecipeType type)
        {
            try
            {
                foreach (var file in this.ToFileList(type)) File.Delete(file);
            }
            catch (Exception ex)
            {
                Logger.Write(this, ex);
            }
        }

        public IRecipeModel GetRecipe(RecipeType type, int no)
        {
            try
            {
                var file = this.ToFile(type, no);
                if (File.Exists(file) == false) return null;

                var res = TS.FW.Serialization.Serialization.JsonDeserializerFile<IRecipeModel>(file);
                if (res == false) return null;

                var info = new FileInfo(file);

                res.Result.CreationTime = info.CreationTime.ToString("yyyy-MM-dd HH:mm:ss");
                res.Result.LastWriteTime = info.LastWriteTime.ToString("yyyy-MM-dd HH:mm:ss");

                return res.Result;
            }
            catch (Exception ex)
            {
                Logger.Write(this, ex);
            }

            return null;
        }

        private bool Exists(RecipeType type, int no) => File.Exists(this.ToFile(type, no));

        private string ToFile(RecipeType type, int no) => Path.Combine(ROOT, type.ToString(), $"{no}.Json");

        private string ToFileName(string path) => Path.GetFileNameWithoutExtension(path);

        private List<string> ToFileList(RecipeType type) => Directory.EnumerateFiles(Path.Combine(ROOT, type.ToString()), "*.json").ToList();

        private int ToRecipeNo(RecipeType type)
        {
            var list = this.ToFileList(type);
            if (list.Count <= 0) return 0;

            return list.Select(t => int.TryParse(this.ToFileName(t), out int no) ? no : -1).Max(t => t) + 1;
        }
    }

    public class UcRecipeModel<T> : IViewModel where T : IRecipeModel
    {
        public RecipeType Type { get; private set; }

        public ObservableCollection<T> RcpList { get; set; } = new ObservableCollection<T>();

        public T RcpSelected { get => this.GetValue<T>(); set => this.SetValue(value); }

        public NormalCommand OnRcpSelectedCmd => new NormalCommand(RcpSelectedCmd);

        public UcRecipeModel(RecipeType type)
        {
            this.Type = type;
        }

        private void RcpSelectedCmd(object param)
        {
            try
            {
                var item = param as T;
                if (item == null) return;

                this.RcpSelected = item;
            }
            catch (Exception ex)
            {
                Logger.Write(this, ex);
            }
        }

        protected override void OnCommand(object commandParameter)
        {
            switch (commandParameter as string)
            {
                case "NAME":
                    {
                        if (this.RcpSelected == null)
                        {
                            AP.System.InterlockMsgEvent("선택 된 레시피가 없습니다.");
                            return;
                        }

                        var name = KeyboardPad.Show();
                        if (string.IsNullOrWhiteSpace(name)) return;

                        if (this.RcpList.Any(t => t.Name == name))
                        {
                            AP.System.InterlockMsgEvent("동일한 이름의 레시피가 존재 합니다.");
                            return;
                        }

                        this.RcpSelected.Name = name;
                    }
                    break;
                case "RELOAD":
                    {
                        this.LoadRecipe();
                    }
                    break;
                case "NEW":
                    {
                        var name = KeyboardPad.Show();
                        if (string.IsNullOrWhiteSpace(name)) return;

                        if (this.RcpList.Any(t => t.Name == name))
                        {
                            AP.System.InterlockMsgEvent("동일한 이름의 레시피가 존재 합니다.");
                            return;
                        }

                        var res = AP.Rcp.NewRecipe(Type, name);
                        if (res == false)
                        {
                            Logger.Write(this, res.Comment, Logger.LogEventLevel.Error);
                            AP.System.InterlockMsgEvent("레시피 생성에 실패하였습니다.");
                            return;
                        }

                        this.LoadRecipe();

                        AP.System.InterlockMsgEvent("레시피 신규생성 되었습니다.");
                    }
                    break;
                case "DELETE":
                    {
                        if (this.RcpSelected == null)
                        {
                            AP.System.InterlockMsgEvent("선택 된 레시피가 없습니다.");
                            return;
                        }

                        // Button 1초 누르면 바로 삭제되게
                        // if (AP.System.InterlockCheckEvent("삭제 하시겠습니까?") == false) return;

                        var res = AP.Rcp.Delete(this.RcpSelected);
                        if (res == false)
                        {
                            Logger.Write(this, res.Comment, Logger.LogEventLevel.Error);
                            AP.System.InterlockMsgEvent("레시피 삭제에 실패하였습니다.");
                            return;
                        }

                        this.LoadRecipe();

                        AP.System.InterlockMsgEvent("레시피 삭제 되었습니다.");
                    }
                    break;

                case "SAVE":
                    {
                        if (this.RcpSelected == null)
                        {
                            AP.System.InterlockMsgEvent("선택 된 레시피가 없습니다.");
                            return;
                        }

                        var res = AP.Rcp.Save(this.RcpSelected);
                        if (res == false)
                        {
                            Logger.Write(this, res.Comment, Logger.LogEventLevel.Error);
                            AP.System.InterlockMsgEvent("레시피 삭제에 실패하였습니다.");
                            return;
                        }

                        this.LoadRecipe();

                        AP.System.InterlockMsgEvent("레시피 저장 되었습니다.");
                    }
                    break;
                case "SAVE AS":
                    {
                        if (this.RcpSelected == null)
                        {
                            AP.System.InterlockMsgEvent("선택 된 레시피가 없습니다.");
                            return;
                        }

                        var name = KeyboardPad.Show();
                        if (string.IsNullOrWhiteSpace(name)) return;

                        if (this.RcpList.Any(t => t.Name == name))
                        {
                            AP.System.InterlockMsgEvent("동일한 이름의 레시피가 존재 합니다.");
                            return;
                        }

                        var res = AP.Rcp.SaveAs(name, this.RcpSelected);
                        if (res == false)
                        {
                            Logger.Write(this, res.Comment, Logger.LogEventLevel.Error);
                            AP.System.InterlockMsgEvent("레시피 생성에 실패하였습니다.");
                            return;
                        }

                        this.LoadRecipe();

                        AP.System.InterlockMsgEvent("레시피 복사 되었습니다.");
                    }
                    break;
            }
        }

        public void ChangedType(RecipeType type)
        {
            try
            {
                if (this.Type == type) return;

                this.Type = type;
                this.LoadRecipe();
            }
            catch (Exception ex)
            {
                Logger.Write(this, ex);
            }
        }

        public void LoadRecipe()
        {
            try
            {
                var no = this.RcpSelected != null ? this.RcpSelected.No : 0;

                this.RcpList.Clear();

                foreach (T item in AP.Rcp.ToRecipeList(Type))
                {
                    if (item == null) continue;

                    this.RcpList.Add(item);
                }

                this.RcpSelected = this.RcpList.FirstOrDefault(t => t.No == no);

                if (this.RcpSelected != null) this.RcpSelected.IsSelcted = true;
            }
            catch (Exception ex)
            {
                Logger.Write(this, ex);
            }
        }
    }
}
