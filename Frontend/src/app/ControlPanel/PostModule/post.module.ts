import { NgModule } from '@angular/core';
import { PostGroupComponent } from './PostGroupComponent/postgroup.component';
import { RouterModule } from '@angular/router';
import { DataTablesModule } from 'angular-datatables';
import { BrowserModule } from '@angular/platform-browser';
import { FormsModule } from '@angular/forms';
import { PostManagerComponent } from './PostManagerComponent/postmanager.component';
import { PostComponent } from './post.component'
import { OverviewPostComponent } from './PostManagerComponent/OverviewPostComponent/overviewpost.component'



@NgModule({
    imports: [
        RouterModule.forChild([
            {
                path: 'post', component: PostComponent, children: [
                    { path: "postgroup", component: PostGroupComponent },
                    { path: "postmanager", component: PostManagerComponent },
                    { path: "postmanager/add", component: OverviewPostComponent },
                    { path: "postmanager/:groupId", component: PostManagerComponent },
                    { path: "postmanager/update/:postId", component: OverviewPostComponent }

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
        OverviewPostComponent
    ],
    bootstrap: [PostGroupComponent],
    providers: [],
})
export class PostModule { }
