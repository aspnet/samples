import { Component, EventEmitter, Input, Output } from '@angular/core';
import { BlazorAdapterComponent } from '../blazor-adapter/blazor-adapter.component';

@Component({
  selector: 'counter',
  template: '',
})

export class CounterComponent extends BlazorAdapterComponent {
  @Input() title: string | null = null;
  @Input() incrementAmount: object | null = null;
  @Input() customObject: object | null = null;
  @Output() customCallback: EventEmitter<any> = new EventEmitter();
}
