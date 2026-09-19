import { ComponentFixture, TestBed } from '@angular/core/testing';
import { HttpClientTestingModule, HttpTestingController } from '@angular/common/http/testing';
import { ToastController } from '@ionic/angular';

import { PointsPage } from './points.page';
import { SalesGoal } from '../../core/models/sales-goal.model';

describe('PointsPage', () => {
  let component: PointsPage;
  let fixture: ComponentFixture<PointsPage>;
  let httpMock: HttpTestingController;

  const mockSalesGoal: SalesGoal = {
    quarter: 3,
    year: 2026,
    pesosGoal: 11_000_000,
    unitsGoal: 6_000,
    executedPesos: 9_000_000,
    executedUnits: 4_500,
    cumplimientoPesosPct: 81.8,
    cumplimientoUnidadesPct: 75,
    puntosPesos: 100,
    puntosUnidades: 150,
    puntosTotal: 250,
    valorTotal: 375_000
  };

  const toastControllerSpy = jasmine.createSpyObj('ToastController', ['create']);
  toastControllerSpy.create.and.resolveTo({
    present: () => Promise.resolve()
  });

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      imports: [PointsPage, HttpClientTestingModule],
      providers: [{ provide: ToastController, useValue: toastControllerSpy }]
    }).compileComponents();

    httpMock = TestBed.inject(HttpTestingController);
    fixture = TestBed.createComponent(PointsPage);
    component = fixture.componentInstance;
  });

  afterEach(() => {
    httpMock.verify();
  });

  function flushInitialRequest(response: SalesGoal | null = mockSalesGoal, opts?: { status: number; statusText: string }) {
    fixture.detectChanges(); // dispara ngOnInit -> GET SalesGoals/current
    const req = httpMock.expectOne((r) => r.url.endsWith('SalesGoals/current'));
    if (opts) {
      req.flush(response, opts);
    } else {
      req.flush(response as SalesGoal);
    }
  }

  it('debería crearse', () => {
    flushInitialRequest();
    expect(component).toBeTruthy();
  });

  it('debería apagar el loading y guardar el resultado al recibir datos', () => {
    flushInitialRequest();

    expect(component.loading()).toBeFalse();
    expect(component.result()).toEqual(mockSalesGoal);
    expect(component.form.value.executedPesos).toBe(mockSalesGoal.executedPesos);
    expect(component.form.value.executedUnits).toBe(mockSalesGoal.executedUnits);
  });

  it('debería apagar el loading aunque la petición inicial falle (regresión del bug de spinner pegado)', () => {
    flushInitialRequest(null, { status: 500, statusText: 'Server Error' });

    expect(component.loading()).toBeFalse();
    expect(component.result()).toBeNull();
  });

  it('el formulario debería empezar inválido si se limpian los campos', () => {
    flushInitialRequest();

    component.form.patchValue({ executedPesos: -1, executedUnits: 0 });
    expect(component.form.invalid).toBeTrue();
  });
});
