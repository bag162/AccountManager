import { Component, OnInit } from '@angular/core';
import { ActivatedRoute } from '@angular/router';
import { ClonService } from '../../../../Services/ClonService'
@Component({
    selector: 'view-clon',
    templateUrl: 'viewclon.component.html'
})

export class ViewClonComponent implements OnInit {
    clonId: number;
    clonData: ClonViewDTO;

    ClonService: ClonService;
    constructor(activateRoute: ActivatedRoute,  ClonService: ClonService) {
        this.clonId = activateRoute.snapshot.params["clonId"];
        this.ClonService = ClonService;
    }

    async ngOnInit() {
        $('#successNot').hide();
        $('#errorNot').hide();
        (await this.ClonService.GetClonInfo(this.clonId)).subscribe({
            next: (data: ClonViewDTO) => {
                this.clonData = data;
            }
        })
    }
}

export class ClonViewDTO {
    Id: number;
    ClonURI: string;
    ClonStatus: string;
    CreateTime: string;
    GroupName: string;
    FillingDataId: number;
}