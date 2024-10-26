
export enum Status {
    UP = 'up',
    DOWN = 'down',
    UNKNOWN = 'UNKNOWN',
}

export interface Site {
    id: number
    url: string,
    status: Status,
    lastChecked: string,
    ping: number,
}

export interface Meta {
    total: number,
    checkStarted: string,
    checksEnded: string,
    intervalActive: boolean,
}