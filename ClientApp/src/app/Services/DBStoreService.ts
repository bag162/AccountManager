import Dexie, { Table } from "dexie";

export interface UserData {
    id?: number;
    Login: string;
}

export interface AccessData {
    id?: number;
    Role: string;
}

export class DBStoreService extends Dexie {
    userData!: Table<UserData, number>;
    accessData!: Table<AccessData, number>;

    constructor() {
        super('ngdexieliveQuery');
        this.version(3).stores({
            userData: '++id',
            accessData: '++id',
        });
    }
}

export const db = new DBStoreService();