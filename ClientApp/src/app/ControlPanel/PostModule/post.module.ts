import { NgModule } from '@angular/core';
import { PostGroupComponent } from './PostGroupComponent/postgroup.component';
import { RouterModule } from '@angular/router';
import { DataTablesModule } from 'angular-datatables';
import { BrowserModule } from '@angular/platform-browser';
import { FormsModule } from '@angular/forms';
import { PostManagerComponent } from './PostManagerComponent/postmanager.component';
import { PostComponent } from './post.component'
import { OverviewPostComponent } from './PostManagerComponent/OverviewPostComponent/overviewpost.component'
import { PostCommentGroupComponent } from './PostCommentComponent/PostCommentGroupComponent/PostCommentGroup.component'
import { PostCommentComponent } from './PostCommentComponent/postcomment.component'
import { ControlPanelComponent } from '../controlpanel.component';
import { GuardService } from 'src/app/Services/GuardService';

@NgModule({
    imports: [
        RouterModule.forChild([
            {
                path: '', component: ControlPanelComponent, children: [
                    {
                        path: 'post', component: PostComponent, canActivate: [GuardService.CanAccess], children: [
                            { path: "group", component: PostGroupComponent, canActivate: [GuardService.CanAccess]  },
                            { path: "manager", component: PostManagerComponent, canActivate: [GuardService.CanAccess]  },
                            { path: "manager/add", component: OverviewPostComponent, canActivate: [GuardService.CanAccess]  },
                            { path: "manager/:groupId", component: PostManagerComponent, canActivate: [GuardService.CanAccess]  },
                            { path: "manager/update/:postId", component: OverviewPostComponent, canActivate: [GuardService.CanAccess]  },
                            { path: "comment/group", component: PostCommentGroupComponent, canActivate: [GuardService.CanAccess]  },
                            { path: "comment/manager", component: PostCommentComponent, canActivate: [GuardService.CanAccess]  },
                            { path: "comment/manager/:groupId", component: PostCommentComponent, canActivate: [GuardService.CanAccess]  }

                        ]
                    }
                ]
            }
        ]),
        DataTablesModule,
        BrowserModule,
        FormsModule
    ],
    exports: [],
    declarations: [
        PostGroupComponent,
        PostManagerComponent,
        PostComponent,
        OverviewPostComponent,
        PostCommentGroupComponent,
        PostCommentComponent
    ],
    bootstrap: [PostGroupComponent],
    providers: [],
})

export class PostModule { }
