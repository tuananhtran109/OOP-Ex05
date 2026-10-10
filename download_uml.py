import urllib.request, urllib.parse

dot = '''
digraph G {
    rankdir=BT;
    node [shape=record, fontname="Arial"];
    
    Device [label="{<<abstract>>\\nDevice|+ DeviceId\\l+ DeviceName\\l|+ CalculateAnnualMaintenanceCost()\\l}"];
    INetworkable [label="{<<interface>>\\nINetworkable|+ IpAddress\\l|+ Connect()\\l}"];
    
    Computer [label="{Computer|+ RamCapacityGB\\l}"];
    Printer [label="{Printer|+ PrinterType\\l}"];
    NetworkPrinter [label="{NetworkPrinter| }"];
    Projector [label="{Projector|+ BrightnessLumens\\l}"];
    
    LabRoom [label="{LabRoom|+ RoomId\\l+ Capacity\\l|+ AddDevice()\\l}"];
    
    Computer -> Device [arrowhead=empty];
    Printer -> Device [arrowhead=empty];
    NetworkPrinter -> Printer [arrowhead=empty];
    Projector -> Device [arrowhead=empty];
    
    Computer -> INetworkable [arrowhead=empty, style=dashed];
    NetworkPrinter -> INetworkable [arrowhead=empty, style=dashed];
    
    LabRoom -> Device [arrowhead=odiamond, style=solid, dir=back, label="Aggregation"];
}
'''

encoded = urllib.parse.quote(dot)
url = f'https://quickchart.io/graphviz?graph={encoded}'

req = urllib.request.Request(url, headers={'User-Agent': 'Mozilla/5.0'})
with urllib.request.urlopen(req) as response, open('diagram.png', 'wb') as out_file:
    out_file.write(response.read())

print('Downloaded diagram.png')
