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

@NgModule({
    imports: [
        RouterModule.forChild([
            {
                path: 'post', component: PostComponent, children: [
                    { path: "group", component: PostGroupComponent },
                    { path: "manager", component: PostManagerComponent },
                    { path: "manager/add", component: OverviewPostComponent },
                    { path: "manager/:groupId", component: PostManagerComponent },
                    { path: "manager/update/:postId", component: OverviewPostComponent },
                    { path: "comment/group", component: PostCommentGroupComponent },
                    { path: "comment/manager", component: PostCommentComponent },
                    { path: "comment/manager/:groupId", component: PostCommentComponent }

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
