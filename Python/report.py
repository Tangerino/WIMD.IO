import json
from wimd import WIMD

w = WIMD()
r = w.login('','')
if r.success:
    print("Access granted")
    reports = w.readReportInfo('2000-01-01T00:00:00', 1)
    if reports.success:
        for report in reports.data:
            print(report['id'], report['name'], report['type'], report['ref_date'])
            r = w.readReportBody(report['id'])
            if r.success:
                reportId = r.data[0]['id']
                reportBody = r.data[0]['report']
                body = json.loads(reportBody)
                print(json.dumps(body,indent=True))
    w.logout()
else:
    print("Can't login")

