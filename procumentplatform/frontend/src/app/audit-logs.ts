// Lightweight stub to satisfy legacy imports during CI builds
// Remove when Angular app is fully retired.

export interface AuditLogEntry {
  timestamp: string;
  user?: string;
  action: string;
  details?: string;
}

export class AuditLogs {
  constructor(public entries: AuditLogEntry[] = []) {}
  add(entry: AuditLogEntry) {
    this.entries.push(entry);
  }
}
