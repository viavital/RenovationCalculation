using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
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
                        if (enteredNewWork != null)
                        {
                            CreatingWork.typeOfWorkName = enteredNewWork.Trim();
                            CreatingWork.quantityHoursOfWork = QuantityOfWork;
                            CreatingWork.CostOfMaterials = enteredCostOfMaterials;
                            if (CreatingWork.typeOfWorkName != "" && CreatingWork.quantityHoursOfWork > 0 && SelectedWorker != null && CreatingWork.CostOfMaterials >=0)
                            {  
                                CreatingWork.WorkerID = SelectedWorker.ID;
                                CreatingWork.TotalCostOfWork = SelectedWorker.PricePerHour * QuantityOfWork + enteredCostOfMaterials;
                                _typeOfWorkService.AddWork(CreatingWork);
                            }
                            else
                            {
                                MessageBox.Show("Inputed wrong data");
                            }
                        }      
                        else
                        {
                            MessageBox.Show("Input name of work");
                        }
                        EnteredNewWork = null;
                        EnteredQuantityOfWork = null;
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
