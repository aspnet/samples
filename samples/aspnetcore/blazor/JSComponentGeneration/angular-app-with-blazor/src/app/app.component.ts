import { Component } from '@angular/core';

@Component({
  selector: 'app-root',
  template: `
    <div class="content">
      <h1>Welcome to {{title}}!</h1>
      <p>This is an Angular application that can also host Blazor components.</p>
      <p>
        <button (click)="addBlazorCounter()">Add Blazor counter</button>
        <button (click)="removeBlazorCounter()">Remove Blazor counter</button>
        <button (click)="modifyParameters()">Modify parameters from JS</button>
      </p>

      <div *ngFor="let counter of blazorCounters">
        <counter
          [title]="counter.title"
          [incrementAmount]="counter.incrementAmount"
          [customObject]="counter.customObject"
          (customCallback)="customCallback($event)">
        </counter>
      </div>
    </div>
  `,
  styles: []
})
export class AppComponent {
  title = 'angular-app-with-blazor';

  blazorCounters: any[] = [];
  nextCounterIndex = 1;

  addBlazorCounter() {
    const index = this.nextCounterIndex++;
    this.blazorCounters.push({
      title: `Counter ${index}`,
      incrementAmount: index,
      customObject: { StringValue: 'Hello!', IntegerValue: 42 }
    });
  }

  removeBlazorCounter() {
    this.blazorCounters.pop();
  }

  modifyParameters() {
    for (const counter of this.blazorCounters) {
      counter.incrementAmount++;

      let { StringValue, IntegerValue } = counter.customObject;

      StringValue += '!';
      IntegerValue -= 1;

      counter.customObject = { StringValue, IntegerValue };
    }
  }

  customCallback(mouseEventArgs: any) {
    console.log(`Custom callback: ${JSON.stringify(mouseEventArgs)}`);
  }
}
