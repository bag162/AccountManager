import { HttpClient } from '@angular/common/http';
import { Component, OnInit } from '@angular/core';
import { ProxyService } from "src/app/Services/ProxyService"
import * as bootstrap from "bootstrap";
import * as $ from 'jquery';
import { environment } from 'src/environments/environment';
import { DataService } from 'src/app/Services/DataService';

@Component({
  selector: 'proxy',
  templateUrl: 'proxy.component.html'
})

export class ProxyComponent implements OnInit {
  proxyGroups: string[];
  dtOptions: any;
  proxyData: string;
  proxyService: ProxyService;
  DataService: DataService;

  constructor(proxyService: ProxyService, DataService: DataService) {
    this.proxyService = proxyService;
    this.DataService = DataService;
  }

  async ngOnInit() {
    $('#proxyFrom').hide();
    $('#successNot').hide();
    $('#errorNot').hide();
    this.dtOptions = {
      ajax: environment.apiUrl + '/api/proxy/get',
      serverSide: true,
      lengthMenu: [[10, 20, 100, 200, -1], [10, 20, 100, 200, "All"]],
      // autoFill: true,
      columns: [{
        title: 'ID',
        data: 'Id'
      }, {
        title: 'Ip',
        data: 'Ip'
      }, {
        title: 'Port',
        data: 'Port'
      }, {
        title: 'Login',
        data: 'Login'
      }, {
        title: 'Password',
        data: 'Password'
      }, {
        title: 'Change ip URI',
        data: 'ChangeIpURI'
      }, {
        title: 'Group',
        data: 'Group'
      }, {
        title: 'Proxy Status',
        data: 'ProxyStatus'
      }],
      select: true,
      dom: 'lBfrtip',
      buttons: [
        'colvis',
        {
          extend: 'copy',
          text: 'Copy selected'
        },
        'print',
        'selectAll',
        {
          text: 'Add proxy',
          key: '1',
          action: function (e, dt, node, config) {
            if ($('#proxyFrom').is(':visible')) {
              $('#proxyFrom').hide(500);
            }
            else {
              $('#proxyFrom').show(500);
            }
          }
        },
        {
          text: 'Delete selected',
          action: function (e, dt, node, config) {
            Delete(dt.rows({ selected: true }).data(), dt);
          }
        },
        {
          text: 'Update selected',
          action: function (e, dt, node, config) {
            ShowUpdateModal(dt.rows({ selected: true }).data(), dt);
          }
        },
        {
          text: 'Reload data',
          action: function (e, dt, node, config) {
            dt.ajax.reload();
          }
        }
      ]
    };

    this.DataService.subscriber$.subscribe(data => {
      this.LoadGroupData();
    });
    await this.LoadGroupData();



    async function Delete(data: string[], dt: any) {
      var deletedProxy = new Array<ProxyDTO>;
      for (let index = 0; index < data.length; index++) {
        const element = data[index];
        var newItem = new ProxyDTO(element["Id"], element["Ip"], element["Port"], element["Login"], element["Password"], element["Group"], element["ProxyStatus"], element["ChangeIpURI"]);
        deletedProxy.push(newItem);
      }
      await ProxyService.DeleteProxy(deletedProxy).subscribe({
        next: (data: boolean) => {
          if (data) {
            $("#successNot").show(200);
            $("#successNot").delay(400).hide(200);
            dt.ajax.reload();
          }
          else {
            $("#errorNot").show(200);
            $("#errorNot").delay(400).hide(200);
          }
        },
        error: error => {
          $("#errorNot").val(error);
          $("#errorNot").show(200);
          $("#errorNot").delay(2000).hide(200);
          $("#errorNot").val("Error");
        }
      })
    }

    async function ShowUpdateModal(data: string[], dt: any) {
      var element = data[0];
      $("#idModal").val(element["Id"]);
      $("#ipModal").val(element["Ip"]);
      $("#portModal").val(element["Port"]);
      $("#loginModal").val(element["Login"]);
      $("#passwordModal").val(element["Password"]);
      $("#statusModal").val(element["ProxyStatus"]);
      $("#changeIpURI").val(element["ChangeIpURI"]);
      new bootstrap.Modal("#proxyModal").show();
    }


  }
  async LoadGroupData() {
    ProxyService.GetProxyGroups().subscribe((data: any) => {
      this.proxyGroups = data;
    })
  }

  async AddProxy() {
    var proxyArray = this.proxyData.split('\n');
    var addedProxy = new Array<ProxyDTO>();
    proxyArray.forEach(element => {
      var elements = element.split(":");
      var changeIpURI: string = "";
      if (elements.length > 4) {
        for (let index = 4; index < elements.length; index++) {
          const element = elements[index];
            changeIpURI = changeIpURI + element + ":";
        }
        changeIpURI = changeIpURI.slice(0, -1);
      }
      else {
        changeIpURI = elements[4]
      }
      var newItem = new ProxyDTO('0', elements[0], elements[1], elements[2], elements[3], $("#groupAddModal").val().toString(), "0", changeIpURI);
      addedProxy.push(newItem);
    });

    (await this.proxyService.AddProxy(addedProxy)).subscribe({
      next: (data: boolean) => {
        if (data) {
          $("#successNot").show(500);
          $("#proxyFrom").hide(500);
          $("#successNot").delay(1500).hide(500);
        }
        else {
          $("#errorNot").show(500);
          $("#proxyFrom").hide(500);
          $("#errorNot").delay(1500).hide(500);
        }
      },
      error: error => {
        $("#errorNot").val(error);
        $("#errorNot").show(200);
        $("#errorNot").delay(2000).hide(200);
        $("#errorNot").val("Error");
      }
    });

  }

  async UpdateProxy() {
    var updatedArray = new Array<ProxyDTO>;
    var updatedProxy = new ProxyDTO($("#idModal").val().toString(), $("#ipModal").val().toString(), $("#portModal").val().toString(), $("#loginModal").val().toString(), $("#passwordModal").val().toString(), $("#groupUpdateModal").val().toString(), $("#statusModal").val().toString(), $("#changeIpURI").val().toString());
    updatedArray.push(updatedProxy);
    await (await ProxyService.UpdateProxy(updatedArray)).subscribe({
      next: (data: boolean) => {
        if (data) {
          $("#successNot").show(200);
          $("#successNot").delay(400).hide(200);
        }
        else {
          $("#errorNot").show(200);
          $("#errorNot").delay(400).hide(200);
        }
      },
      error: (error: any) => {
        $("#errorNot").val(error);
        $("#errorNot").show(200);
        $("#errorNot").delay(2000).hide(200);
        $("#errorNot").val("Error");
      }
    })
  }
}

export class ProxyDTO {
  constructor(id: string, ip: string, port: string, login: string, password: string, group: string, ProxyStatus: string, ChangeIpURI: string) {
    this.id = id;
    this.Ip = ip;
    this.Port = port;
    this.Login = login;
    this.Password = password;
    this.Group = group;
    this.ProxyStatus = ProxyStatus;
    this.ChangeIpURI = ChangeIpURI;

  }
  public id: string;
  public Ip: string;
  public Port: string;
  public Login: string;
  public Password: string;
  public Group: string;
  public ProxyStatus: string;
  public ChangeIpURI: string;
}
