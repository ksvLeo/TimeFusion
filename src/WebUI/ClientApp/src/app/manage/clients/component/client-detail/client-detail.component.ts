import { Component, OnInit } from '@angular/core';
import { ActivatedRoute, Router } from '@angular/router';
import { NgbModal } from '@ng-bootstrap/ng-bootstrap';
import { ActionInfo } from 'src/app/commons/classes/action-info';
import { FieldFormat, FieldInfo, GridConfiguration } from 'src/app/commons/classes/grid-configuration';
import { ModalInfo } from 'src/app/commons/classes/modal-info';
import { PaginatedList } from 'src/app/commons/classes/paginated-list';
import { ContactManagementUrlParams } from 'src/app/commons/classes/contactManagementUrlParams';
import { GenericModalComponent } from 'src/app/commons/components/generic-modal/generic-modal.component';
import { ClientClient, ClientDto, ContactClient, ContactDto, CurrencyDto, DeleteClientResult, ReactivateClientResult } from 'src/app/web-api-client';
import { ToastrService } from 'ngx-toastr';
import { ModeParameter } from 'src/app/shared/enums/modeParameter';
@Component({
  selector: 'app-client-detail',
  templateUrl: './client-detail.component.html',
  styleUrls: ['./client-detail.component.css']
})
export class ClientDetailComponent implements OnInit {

  clientInfo: ClientDto = new ClientDto({ currency: new CurrencyDto() })
  closeModal: any;
  paginatedList: PaginatedList<ContactDto>;
  tableConfig: FieldInfo[];
  configurationInfo: GridConfiguration;
  actionList: ActionInfo[] = []
  modalInfo: ModalInfo;
  result: DeleteClientResult;
  projectList = [1,2,3,4,5]

  constructor(private clientService: ClientClient,
              
              private router: Router,
              private route: ActivatedRoute,
              private modalService: NgbModal,
              private contactService: ContactClient,
              private toasterService: ToastrService) { }

  ngOnInit(): void {
    this.getClient();
    this.configurationGrid();
    this.loadActions()
  }

  onEditClient(){
    let queryParams = new ContactManagementUrlParams()
    queryParams.mode = 2;
    queryParams.id = this.clientInfo.id.toString()
    this.router.navigate(['/manage/client', queryParams])
  }

  onDeactivateClient() {
    let modalInfo = new ModalInfo()
    modalInfo.title = "Deactivate Client?"
    modalInfo.message = "Are you ready to finish your work with " + this.clientInfo.name + "?"  
    this.openModal(modalInfo).then(res => {
      if (res == "accept") {
        this.clientService.deleteClient(this.clientInfo.id).subscribe((response: any) => {
          switch (response) {
            case DeleteClientResult.Success:
              this.toasterService.success(`The client ${this.clientInfo.name} was successfully deactivated.`);
              this.getClient();

              break;
            case DeleteClientResult.Error_ActiveProjects:
              this.toasterService.warning("An error occurred when deactivating client, you have active projects.");
              break;
            default:
              this.toasterService.error("An error occurred while deactivating the client.");
              break;
          }
        });
      }
    });
  }

  onReactivateClient() {
    let modalInfo = new ModalInfo;
    modalInfo.title = "Reactivate Client?"
    modalInfo.message = "Are you ready to start working with " + this.clientInfo.name + " again?"  
    this.openModal(modalInfo).then(input => {
      if (input == "accept") {
        this.clientService.reactivateClient(this.clientInfo.id).subscribe(response => {

          switch(response){
            case ReactivateClientResult.Success:
              this.getClient()
              this.toasterService.success(`The client ${this.clientInfo.name} has been successfully reactivated.`);
              break;
            case ReactivateClientResult.Error_NotFound:
              this.toasterService.warning(`Unable to faind the client with ID #${this.clientInfo.id}.`);
              break;
            case ReactivateClientResult.Error_AlreadyActive:
              this.toasterService.warning("The client is already active.");
              break;
            default:
              this.toasterService.error("An error occurred while reactivating the client.");
              break    
          }
        }, err => {
          this.toasterService.error("An error occurred while reactivating the client.");
        });
      } else {}
    })
  }

  onAddContact(){
    let urlParams = new ContactManagementUrlParams()
    urlParams.mode = 1;
    urlParams.id = this.clientInfo.id.toString()
    this.router.navigate(['/manage/client/contact', urlParams]);

  }

  onReportContact(item: ContactDto) {
    let mailText = "mailto:"+ item.email +"?subject=files&body=bodyodyody"; // add the links to body
    window.location.href = mailText;
  }

  onEditContact(item: ContactDto) {
    let urlParams = new ContactManagementUrlParams()
    urlParams.mode = 2;
    if(item != null)
    urlParams.id = item.clientId.toString()
    urlParams.contactId = item.id
    this.router.navigate(['/manage/client/contact', urlParams]); 
  }
  
  onFlagContact(item: ContactDto) {
    this.modalInfo = new ModalInfo()
    if(item.active) {
      this.modalInfo.title = "Deactivate contact?"
      this.modalInfo.message = "Are you sure you want to deactivate " + item.name + "?"
        this.openModal(this.modalInfo).then((result: any) => {
            if (result == "accept") {
              this.contactService.deleteContact(item.clientId, item.id).subscribe((res: any) => {
                    this.toasterService.success("Contact " + item.name + " succesfully deactivated.");
                    this.getClient();
              },
              (error: any) => {
              })
            }
        }, (reason:any) => {
          
        });
    } else {
      this.modalInfo.title = "Reactivate contact?"
      this.modalInfo.message = "Are you sure you want to reactivate " + item.name + "?"
        this.openModal(this.modalInfo).then((result:any) => {
            if (result == "accept") {
              this.contactService.reactivateContact(item.id, item.clientId).subscribe((res: any) => {
                  this.modalInfo = new ModalInfo()
                  this.modalInfo.title = "Contact flagged"
                  this.modalInfo.message = "Contact " + item.name + " succesfully reactivated."
                  this.openModal(this.modalInfo)
                  this.getClient()
              },
              (error: any) => {
                this.modalInfo = new ModalInfo()
                this.modalInfo.title = "We couln't do that :("
                this.modalInfo.message = "An error ocurred.\n" + error
                this.openModal(this.modalInfo)
              })
            }
        }, (reason:any) => {
          
        });
    }
  }

  getClient() {
    this.clientService.getClient(Number(this.route.snapshot.paramMap.get('id'))).subscribe((res: ClientDto) => {
      this.clientInfo = res
      this.paginatedList = new PaginatedList<ContactDto>()
      this.paginatedList.items = res.contactList
      this.paginatedList.totalCount = res.contactList.length
                                       
    })
  }

  configurationGrid() {
    this.tableConfig = [
      new FieldInfo("Name", "name", FieldFormat.text, true),
      new FieldInfo("Title", "title", FieldFormat.text, true),
      new FieldInfo("Email", "email", FieldFormat.text, true),
      new FieldInfo("Phone", "phoneNumber", FieldFormat.text, true),
      new FieldInfo("Active", "active", FieldFormat.text, true)
  ];
    this.configurationInfo = new GridConfiguration(this.tableConfig)
  }

  loadActions() {
    this.actionList = []

    let action = new ActionInfo();
    action.label = "Report Contact";
    action.enable = true;
    action.event.subscribe(item => this.onReportContact(item));
    this.actionList.push(action);

    action = new ActionInfo();
    action.label = "Edit Contact";
    action.enable = true;
    action.event.subscribe(item => this.onEditContact(item));
    this.actionList.push(action);

      action = new ActionInfo();
      action.label = "Flag Contact";
      action.enable = true;
      action.event.subscribe(item => this.onFlagContact(item));
      this.actionList.push(action);
  }

  openModal(modalInfo: ModalInfo): Promise<string> {
      var promise = new Promise<string>((resolve) => {
        setTimeout(() => {
          var modalRef = this.modalService.open(GenericModalComponent, { modalDialogClass: "", centered: true})
          modalRef.componentInstance.title = modalInfo.title;
          modalRef.componentInstance.message = modalInfo.message;
          modalRef.result.then((result:any) => {
          resolve(result)}, (reason:any) => {
          });
        }, 150);
    });
    return promise;
  }

}
