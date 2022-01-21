using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using RenovationCalculation.Model;

namespace RenovationCalculation.ApplictionViewModel
{
   partial class StackOfAddingWorksViewModel
    {
        private RelayCommand addWorkCommand;
        public RelayCommand AddWorkCommand
        {
            get
            {
                return addWorkCommand ??
                    (addWorkCommand = new RelayCommand(_ =>
                    {
                        TypeOfWorkModel CreatingWork = new();
                        CreatingWork.typeOfWorkName = enteredNewWork;
                        CreatingWork.quantityHoursOfWork = enteredQuantityOfWork;
                        CreatingWork.CostOfMaterials = enteredCostOfMaterials;
                        CreatingWork.WorkerID = SelectedWorker.ID;
                        CreatingWork.TotalCostOfWork = SelectedWorker.PricePerHour * EnteredQuantityOfWork + enteredCostOfMaterials;

                        _typeOfWorkService.AddWork(CreatingWork);

                        EnteredNewWork = null;
                        EnteredQuantityOfWork = 0;
                        EnteredCostOfMaterials = 0;
                        SelectedWorker = null;
                    }));
            }
        }
        private RelayCommand editWorkCommand;
        public RelayCommand EditWorkCommand
        {
            get
            {
                return editWorkCommand ??
                    (editWorkCommand = new RelayCommand(_ =>
                    {
                        SelectedWork.TotalCostOfWork = WorkerOnSelectedWork.PricePerHour * SelectedWork.quantityHoursOfWork + selectedWork.CostOfMaterials;

                        _typeOfWorkService.UpdateWork(SelectedWork);

                        SelectedWorker = null;
                    }));
            }
        }

        private RelayCommand removeWorkCommand;
        public RelayCommand RemoveWorkCommand
        {
            get
            {
                return removeWorkCommand ??
                    (removeWorkCommand = new RelayCommand(obj =>
                    {
                        var workToRemove = obj as TypeOfWorkModel;
                        if (workToRemove != null)
                        {
                            _typeOfWorkService.DeleteWork(workToRemove);
                        }
                    }));
            }
        }
    }
}
